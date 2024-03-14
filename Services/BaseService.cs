using Models.Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BaseService
    {
        protected readonly WorkJournalContext db;

        public BaseService (WorkJournalContext dbContext)
        {
            db = dbContext;
        }

        public SaveChangesResult DbSaveChanges()
        {
            var result = new SaveChangesResult(false, "");

            try
            {
                db.SaveChanges();
                result.IsSuccess = true;

                return result;
            }
            catch (Exception ex)
            {
                //todo 秀出錯誤訊息
                return result;
            }

        }
    }
}
