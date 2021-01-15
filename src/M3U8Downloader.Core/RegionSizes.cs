namespace M3U8Downloader.Core
{
    public static class RegionSizes
    {
        public const double MenuWidth_Size = 600;
        public const double MenuHeight_Size = 32;

        public const double TaskListWidth_Size = 200;
        public const double TaskListHeight_Size = 450;

        public const double DetailPageWidth_Size = MenuWidth_Size - TaskListWidth_Size;//400
        public const double DetailPageHeight_Size = TaskListHeight_Size;//450

        public const double SettingWidth_Size = 0;
        public const double SettingHeight_Size = 0;

        public const double MainWindowContentWidth_Size = MenuWidth_Size;//600
        public const double MainWindowContentHeight_Size = MenuHeight_Size + TaskListHeight_Size;//482
    }
}
