drop table if exists image;
drop table if exists category;


create table image(
	image_id SERIAL Primary key,
	name text,
	imageString text,
	fk_categoryID int,
	constraint fk_category
		foreign key(fk_categoryID)
			references category(categoryID)
);

create table category(
	categoryID SERIAL Primary Key ,
	category text
);

insert into category (category) values ('Möbel')

select i_add_Table_Data('bär','Tier','djdsdjjd');

create or replace function i_add_Table_Data(imagename text, kategory text, base64string text)
	returns void
	LANGUAGE plpgsql
as $$
declare
	str text;
	cat_id int4;
	base64InTable text;
begin
	
	select categoryid into cat_id from category c where category = kategory;

	if(cat_id is null) then	
		str := 'insert into public.category(category) values(''' || kategory || ''')';
		
		
		
		raise notice 'str: %',str;
		execute str;
		
		select categoryid into cat_id from category c where category = kategory;
	end if;

	raise notice 'cat_id: %',cat_id;

	select imagestring into base64InTable from image i where imagestring = base64string;

	if(base64InTable is null) then
	 	insert into image  (name,fk_categoryid,imagestring) values(imagename,cat_id,base64string);
	 	raise notice '%', 'Fehler';
	ELSE 
		update image set name = imagename, fk_categoryid = cat_id where imagestring = base64string;
	end if;
end;
$$;

select i_get_Table_categories();


drop function i_get_Table_categories()

create or replace function i_get_Table_categories()
	--RETURNS text[]
	returns table(tabledata text)
	LANGUAGE plpgsql
as $$
declare
	str text[];
begin
	--str := array(select category as str from category);
	--SELECT ARRAY(SELECT ROW(category) FROM category) as str;
	--str := array( select category from category );

	RETURN QUERY
		select category from category;
	
	
	
	--raise notice '%',str;
	--return str;
	--return query
		--SELECT str;
	
end;
$$;

drop function i_get_int_isNameAlreadyInCategory

select i_get_int_isNameAlreadyInCategory('bear','animal');

create or replace function i_get_int_isNameAlreadyInCategory(imageName text, iCategory text)
	--RETURNS int
	returns boolean
	LANGUAGE plpgsql
as $$
declare
	str text;
	i int = 0;
	b boolean=false;
begin
	select name into str from image inner join category on (image.fk_categoryid = category.categoryid) where category.category = iCategory and name like imageName;
	
	if(str = imageName) then
		i=1;
		b=true;
	end if;

	--return i;
	return b;
	
end;
$$;

select i_get_Table_NamesInCategory('animal')

create or replace function i_get_Table_NamesInCategory(iCategory text)
	returns table (names text)
	LANGUAGE plpgsql
as $$
declare
	str text;
begin
	return query 
		select name from image inner join category on (image.fk_categoryid = category.categoryid) where category.category = iCategory;

end;
$$;

select i_get_ImageString('bear','animal')

create or replace function i_get_ImageString(imageName text, iCategory text)
	returns text
	LANGUAGE plpgsql
as $$
declare
	str text;
begin
	
	select imagestring into str from image inner join category on (image.fk_categoryid = category.categoryid) where category.category = iCategory and image."name" = imageName ;
	
	return str;
	
end;
$$;


