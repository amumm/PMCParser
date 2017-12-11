select * from Article_Status;
select Count(*) from Article_Status
WHERE Valid = '3' AND Downloaded = '0';

update Article_Status
SET To_Analyze = '1'
WHERE Valid = '1';

select * from Reference_Keywords;

delete from Reference_Keywords
where keyword = 'Review';

Delete FROM Data_Types
WHERE Data_Type = 'y' AND Key_Word = 'x';

select * from keywords;

select * from Data_Types;

select Distinct Data_Type from Data_Types;

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