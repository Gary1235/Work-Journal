using Models.Model;
using Models.ViewModel;

namespace Services
{
    public interface IWrokScheduleService
    {
        List<ScheduleViewModel> GetList();

        ScheduleItemViewModel GetData(Guid id);
    }


    public class WorkScheduleService : IWrokScheduleService
    {
        public List<ScheduleViewModel> GetList()
        {
            var list = new List<ScheduleViewModel>();

            // todo 實作邏輯
            using (var db = new WorkJournalContext())
            {
                
            }

            return list;
        }

        public ScheduleItemViewModel GetData(Guid id)
        {
            var data = new ScheduleItemViewModel();

            // todo 實作邏輯

            return data;
        }
    }
}
