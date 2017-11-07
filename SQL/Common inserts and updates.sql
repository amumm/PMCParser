select * from keywords;

select Count(*) from Stored_Articles;

select * from Stored_Articles;

select * from Articles_To_Download;
select Count(*) from Articles_To_Download;

select * from keywords
WHERE keyword = '';

select * from Data_Types;

select * from Data_Types
where Data_Type = '';

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
		AND filename = 'y')



        
        