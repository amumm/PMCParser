select * from Article_Status;
select Count(*) from Article_Status
WHERE Valid = '0';

select * from Reference_Keywords;

select * from keywords;

select * from Data_Types;

INSERT INTO Data_Types (Key_Word, Data_Type)
	SELECT 'x', 'y'
	FROM dual
	WHERE NOT EXISTS 
		(SELECT *  FROM Data_Types
		WHERE Key_Word = 'x'
		AND Data_Type = 'y');
                    
INSERT INTO keywords(keyword, filename) 
    SELECT 'x', 'y'
	FROM dual
	WHERE NOT EXISTS 
		(SELECT * FROM keywords
		WHERE keyword = 'x'
		AND filename = 'y');                 