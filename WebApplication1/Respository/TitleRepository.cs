using static Zenex.Master.Models.Master;
using Zenex.Master.IRespository;
using Zenex.DBContext;

namespace Zenex.Master.Respository
{
    public class TitleRepository : ITitleRepository
    {
        private readonly ZenexContext _dbContext;

        public TitleRepository(ZenexContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPTitle> GetAllTitles()
        {
            try
            {
                return _dbContext.CBPTitles.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPTitle> CreateTitle(CBPTitle Title)
        {
            try
            {
                Title.IsActive = true;
                Title.CreatedOn = DateTime.Now;
                var result = _dbContext.CBPTitles.Add(Title);
                await _dbContext.SaveChangesAsync();
                return Title;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPTitle> UpdateTitle(CBPTitle Title)
        {
            try
            {
                var entity = _dbContext.Set<CBPTitle>().FirstOrDefault(x => x.Title == Title.Title && x.Language == Title.Language);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Title).State = EntityState.Modified;
                entity.TitleText = Title.TitleText;
                entity.ModifiedBy = Title.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Title;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPTitle> DeleteTitle(CBPTitle Title)
        {
            try
            {
                //var entity = await _dbContext.Set<CBPTitle>().FindAsync(Title.Title, Title.Language);
                var entity = _dbContext.Set<CBPTitle>().FirstOrDefault(x => x.Title == Title.Title && x.Language == Title.Language);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<CBPTitle>().Remove(entity);
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
