class PageList {
    PageCount: 0;
    PageSize: 10;
    CurrentPage: 1;
    HasPreviousPage: false;
    HasNextPage: false;
    IsFirstPage: false;
    IsLastPage: false;
}

class ScheduleViewModel {
    Id: '';
    Subject: '';
    WorkDateTime: '';
    CreateDateTime: '';
    UpdateDateTime: '';
}

class ScheduleItemViewModel {
    Id: '';
    ScheduleId: '';
    WorkItem: '';
    WorkDuration: '';
}

class WorkSchedulePageList extends PageList {
    List: ScheduleViewModel[];
}