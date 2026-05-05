// using System.Reactive;
// using ReactiveUI;
using knotless.services;

namespace knotless.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "welcome to knotless!";

    // method that will be called by the button
    public void StartWeavingCommand()
    {
        var config = ConfigLoader.Load();
        var engine = new WeaverEngine();
        engine.StartWeaving(config);
    }
}