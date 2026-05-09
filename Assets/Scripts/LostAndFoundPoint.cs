public class LostAndFoundPoint : BaseInteractable
{
    protected override void Interact()
    {
        WorldManager.Instance.Return();
    }
}