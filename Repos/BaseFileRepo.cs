namespace backend_aoz.Repos;

public abstract class BaseFileRepo {
    private const string dbFolder = "./db";
    protected abstract string dbFileName { get; }
    private static object padlock = new object();
    protected string dbFullPath => $"{dbFolder}/{dbFileName}";
    public BaseFileRepo()
    {
        if (!Directory.Exists(dbFolder))
        {
            Directory.CreateDirectory(dbFolder);
        }
        if (!File.Exists(dbFullPath))
        {
            File.Create(dbFullPath);
        }
    }
}