create or replace TYPE                   "EVENTTYPE" AS OBJECT (
	"AGGREGATEID" varchar2(50),
    "VERSION" INTEGER,
	"DATA" blob,	
	"DATE" timestamp
)


create or replace TYPE                   "EVENTSTYPE" AS TABLE OF "SYSTEM"."EVENTTYPE";

