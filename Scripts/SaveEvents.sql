create or replace PROCEDURE                   "SAVEEVENTS" (
    AggregateId in varchar2,
    ExpectedVersion in INTEGER,
	Eventss in "SYSTEM"."EVENTSTYPE"
)
AS   
    CurrentVersion Integer;
BEGIN
	SET TRANSACTION NAME 'save_aggregate_events';

    CurrentVersion := NULL;
    FOR v IN (SELECT "Version" into CurrentVersion FROM "SYSTEM"."EventSource" WHERE "AggregateId" = AggregateId) LOOP
      CurrentVersion := v."Version";
      EXIT;
    END LOOP;

	IF CurrentVersion IS NULL THEN
		CurrentVersion := 0;
		INSERT INTO "EventSource"("AggregateId", "Version") VALUES (AggregateId, CurrentVersion);
	END IF;

	-- concurrency validation
	IF ExpectedVersion - 1 != CurrentVersion THEN
		RAISE_APPLICATION_ERROR(-20001,'Concurrency problem');
    END IF;

	FOR R IN Eventss.FIRST .. Eventss.LAST
	loop
	    IF AggregateId != Eventss(R)."AGGREGATEID" THEN
	    	RAISE_APPLICATION_ERROR(-20001,'Events for only one aggregate are processed at the time');
        END IF;
        
		CurrentVersion := CurrentVersion + 1;
		INSERT INTO "SYSTEM"."Event"("AggregateId", "Data", "Version", "Date") VALUES (Eventss(R)."AGGREGATEID", Eventss(R)."DATA", CurrentVersion, Eventss(R)."DATE"); 
 	end loop;

    UPDATE "SYSTEM"."EventSource"
 	SET "Version" = CurrentVersion
 	WHERE "AggregateId" = AggregateId;

	COMMIT;

	EXCEPTION WHEN OTHERS THEN
	BEGIN
    DBMS_OUTPUT.PUT_LINE('rollback');
   		ROLLBACK;
	END;
	
END;