select Count(*) from Article_Status;
select * from Article_Status;

select * from Articles_To_Download;
select Count(*) from Articles_To_Download;

select * from Articles_To_Analyze;
select Count(*) from Articles_To_Analyze;

select PMC_Id from Articles_To_Download
Where PMC_Id not in (select (PMC_Id)
					from Article_Status where
                    Downloaded = 1);

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
		AND filename = 'y');



select * from test;

INSERT INTO Articles_To_Download(PMC_Id)
			SELECT 10 FROM dual
				WHERE NOT EXISTS(SELECT 1 FROM Articles_To_Download WHERE PMC_Id = 10)
				AND NOT EXISTS(SELECT 1 FROM Article_Status WHERE PMC_Id = 10);

	
INSERT INTO Articles_To_Download(PMC_Id) 
            SELECT a.PMC_Id FROM Articles_To_Analyze a
                WHERE NOT EXISTS(SELECT d.PMC_Id  FROM Articles_To_Download d
                    WHERE a.PMC_Id = d.PMC_Id)
                    AND NOT EXISTS(SELECT s.PMC_Id FROM Article_Status s WHERE a.PMC_Id = s.PMC_Id);
                    
                    