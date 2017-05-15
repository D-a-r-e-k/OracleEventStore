using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace OracleEventStore.Domain
{
    public class Event : INullable, IOracleCustomType
    {
        private bool _isNull;

        [OracleObjectMappingAttribute("AGGREGATEID")]
        public string AggregateId;

        [OracleObjectMappingAttribute("VERSION")]
        public int Version;

        [OracleObjectMappingAttribute("DATA")]
        public byte[] Data;

        [OracleObjectMappingAttribute("DATE")]
        public DateTime Date;

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "VERSION", Version);
            OracleUdt.SetValue(con, pUdt, "DATA", Data);
            OracleUdt.SetValue(con, pUdt, "DATE", Date);
            OracleUdt.SetValue(con, pUdt, "AGGREGATEID", AggregateId);         
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            AggregateId = (string) OracleUdt.GetValue(con, pUdt, "AGGREGATEID");
            Version = (int)OracleUdt.GetValue(con, pUdt, "VERSION");
            Data = (byte[])OracleUdt.GetValue(con, pUdt, "DATA");
            Date = (DateTime)OracleUdt.GetValue(con, pUdt, "DATE");
        }
       
        public bool IsNull => _isNull;

        public static Event Null => new Event {_isNull = true};
    }

    [OracleCustomTypeMappingAttribute("SYSTEM.EVENTTYPE")]
    public class EventFactory : IOracleCustomTypeFactory
    {
        public IOracleCustomType CreateObject()
        {
            return new Event();
        }
    }
}
