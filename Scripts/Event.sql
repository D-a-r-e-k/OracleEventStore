
  CREATE TABLE "SYSTEM"."Event" 
   (	"AggregateId" VARCHAR2(50 BYTE), 
	"Data" BLOB NOT NULL ENABLE, 
	"Version" NUMBER(*,0) NOT NULL ENABLE, 
	"Date" DATE NOT NULL ENABLE, 
	 CONSTRAINT "pk_Event" PRIMARY KEY ("AggregateId", "Version")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSTEM"  ENABLE, 
	 CONSTRAINT "Event_EventSource" FOREIGN KEY ("AggregateId")
	  REFERENCES "SYSTEM"."EventSource" ("AggregateId") ENABLE
   )