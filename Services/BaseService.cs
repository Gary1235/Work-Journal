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
            var result = new SaveChangesResult(false, "更新失敗");

            try
            {
                db.SaveChanges();
                result.IsSuccess = true;
                result.Message = "更新成功";
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
