using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBConcurrencyManagement.Models;

namespace DBConcurrencyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private static readonly SemaphoreSlim _semaphore = new(initialCount: 1, maxCount: 1);
        private readonly BankDbContext _context;

        public BankAccountsController(BankDbContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(long id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            return bankAccount;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(long id, BankAccount bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(bankAccount);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? bankAccount : NoContent();
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankAccount(long id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool BankAccountExists(long id)
        {
            return _context.BankAccounts.Any(e => e.Id == id);
        }

        
        //Update Account Balance 
        [HttpPut("Deposit/{id}")]
        public async Task<string> Deposit(long id, decimal amount)
        {
            await _semaphore.WaitAsync();
            var bankAccount = await _context.BankAccounts.FindAsync(id);
                bankAccount!.Balance += amount;
            var result = await _context.SaveChangesAsync();
            _semaphore.Release();

            return result > 0 ? "Successfully Deposited" : "Could not deposit";
           
        }
    }
}
