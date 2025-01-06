using Zenex.DBContext;
using Zenex.Master.IRespository;
using static Zenex.Master.Models.Master;


namespace Zenex.Master.Respository
{
    public class RegisterBankRepository : IRegisterBankRepository
    {
        private readonly ZenexContext _dbContext;

        public RegisterBankRepository(ZenexContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPBank> GetAllBanks()
        {
            try
            {
                return _dbContext.CBPBanks.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CBPBank GetBankByIFSC(string IFSC)
        {
            try
            {
                return _dbContext.CBPBanks.FirstOrDefault(x => x.BankCode == IFSC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPBank> CreateBank(CBPBank Bank)
        {
            try
            {
                Bank.IsActive = true;
                Bank.CreatedOn = DateTime.Now;
                var result = _dbContext.CBPBanks.Add(Bank);
                await _dbContext.SaveChangesAsync();
                return Bank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPBank> UpdateBank(CBPBank Bank)
        {
            try
            {
                var entity = _dbContext.Set<CBPBank>().FirstOrDefault(x => x.BankCode == Bank.BankCode);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Bank).State = EntityState.Modified;
                entity.BankName = Bank.BankName;
                entity.BankCity = Bank.BankCity;
                entity.BankCountry = Bank.BankCountry;
                entity.ModifiedBy = Bank.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Bank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPBank> DeleteBank(CBPBank Bank)
        {
            try
            {
                //var entity = await _dbContext.Set<CBPBank>().FindAsync(Bank.Bank, Bank.Language);
                var entity = _dbContext.Set<CBPBank>().FirstOrDefault(x => x.BankCode == Bank.BankCode);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<CBPBank>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
