using System.Text.Json;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;

namespace Hellclient.Core.Features.Repos;

public interface IUserPasswordRepo
{
    public UserPassword? Load();
    public void Save(UserPassword userPassword);
}

public class UserPasswordRepo : IUserPasswordRepo
{
    public UserPasswordRepo(string filepath)
    {
        this.FilePath = filepath;
    }
    public string FilePath { get; init; }

    public UserPassword? Load()
    {
        if (!File.Exists(FilePath))
        {
            return null;
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<UserPassword>(json, JsonContext.Instance.UserPassword);
    }

    public void Save(UserPassword userPassword)
    {
        var json = JsonSerializer.Serialize(userPassword, JsonContext.Instance.UserPassword);
        File.WriteAllText(FilePath, json);
    }
}
