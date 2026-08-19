using System.Collections;

public interface ISpawner<TData>
{
    IEnumerator Spawn(TData data);
}
