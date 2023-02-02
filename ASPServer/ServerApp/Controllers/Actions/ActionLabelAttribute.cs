namespace ServerApp.Controllers.Actions;

public class ActionLabelAttribute : Attribute
{
    private string actionName;
    public ActionLabelAttribute(string actionName)
    {
        this.actionName = actionName;
    }
}