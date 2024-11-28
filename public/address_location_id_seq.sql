create sequence address_location_id_seq;

alter sequence address_location_id_seq owner to postgres;

alter sequence address_location_id_seq owned by "Address".location_id;

