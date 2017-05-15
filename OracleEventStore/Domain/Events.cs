
using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace OracleEventStore.Domain
{
    public class Events : INullable, IOracleCustomType
    {
        private bool _isNull;

        [OracleArrayMapping()]
        public Event[] Value;

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, Value);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            Value = (Event[]) OracleUdt.GetValue(con, pUdt, 0);
        }

        public bool IsNull => _isNull;

        public static Events Null => new Events {_isNull = true};
    }

    [OracleCustomTypeMapping("SYSTEM.EVENTSTYPE")]
    public class EventsFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
    {
        public IOracleCustomType CreateObject()
        {
            return new Events();
        }

        public Array CreateArray(int numElems)
        {
            return new Event[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return null;
        }
    }
}
