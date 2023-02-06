namespace MusicStore.Entities;

public class EntityBase
{
    //[Key] // para cuando quieres especificar si la llave tiene otro nombre
    public int Id { get; set; }
    public bool Status { get; set; }

    protected EntityBase()
    {
        Status = true;
    }
}