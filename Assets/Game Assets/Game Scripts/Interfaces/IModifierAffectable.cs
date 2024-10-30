using System.Collections;

public interface IModifierAffectable
{
    public ModifierEffect currentEffect { get; set; }
    
    public void StartEffect();
    public IEnumerator TickEffect();
}