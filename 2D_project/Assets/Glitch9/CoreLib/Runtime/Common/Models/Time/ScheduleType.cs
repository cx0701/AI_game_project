namespace Glitch9
{
    public enum ScheduleType
    {
        /// <summary>
        /// 월화수목금토일 중 선택
        /// Every 1 Week, Every 2 Weeks~
        /// </summary>
        Weekly,
        /// <summary>
        /// Every 1 Day (Everyday), Every 2 Days~
        /// </summary>
        Daily,
        /// <summary>
        /// Selected Days in the month (1~31)
        /// Every 1 Month, Every 2 Months~
        /// </summary>
        Monthly,
    }
}