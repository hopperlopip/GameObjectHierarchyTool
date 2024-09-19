namespace GameObjectHierarchyTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (args.Length > 0)
            {
                string arg = args[0];
                if (!arg.EndsWith(".gh"))
                    return;
                byte[] fileData = File.ReadAllBytes(arg);
                GameObjectHierarchy gameObjectHierarchy;
                try
                {
                    gameObjectHierarchy = GameObjectHierarchyFile.Deserialize(fileData);
                }
                catch { return; }
                string ghFileName = arg;
                var ghEditorForm = new GhEditorForm(ghFileName, gameObjectHierarchy);
                Application.Run(ghEditorForm);
            }
            else
                Application.Run(new MainWindow());
        }
    }
}