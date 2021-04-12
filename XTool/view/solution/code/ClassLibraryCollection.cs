

namespace JunkHarness
{
    using System.Collections.ObjectModel;

    public class ClassLibraryCollection : KeyedCollection<string,ClassLibrary>
    {

        protected override string GetKeyForItem(ClassLibrary item)
        {
            return item.Namespace;
        }


    }
}
