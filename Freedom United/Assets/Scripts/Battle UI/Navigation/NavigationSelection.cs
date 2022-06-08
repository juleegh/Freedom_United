public class NavigationSelection
{
    protected int currentIndex;
    public int CurrentIndex { get { return currentIndex; } }
    protected virtual int MaxElement { get { return 0; } }

    public virtual void Next()
    {

    }

    public virtual void Previous()
    {

    }

    public void CleanSelection()
    {
        currentIndex = 0;
    }
}