using System;
using System.Collections;

[assembly: CLSCompliant(false)]
namespace SPCAFContrib.Demo.Workflow.ExecuteStoredProcedure
{
    public static class Utils
    {
  
        #region Conversion Functions
        public static object[] ICollectionToObjectArray(ICollection coll)
        {
            if (coll == null) return new object[0];

            object[] obj = new object[coll.Count];
            ArrayList al = new ArrayList(coll);
            int i = 0;
            foreach (object objAl in al)
            {
                obj[i] = objAl;
                i++;
            }
            return obj;
        }

        public static IDictionary ArrayListsToIDictionary(ArrayList alKeys, ArrayList alValues)
        {
            if (alKeys == null) return new Hashtable();
            if (alValues == null) return new Hashtable();

            else
            {
                int iCounter = 0;
                Hashtable ht = new Hashtable(alKeys.Count);
                for (int i = 0; i < alKeys.Count; i++)
                {
                    object key = alKeys[i];
                    object value = alValues[i];
                    ht[key] = value;
                }
                return ht;
            }
        }
        #endregion

        
    }
}

