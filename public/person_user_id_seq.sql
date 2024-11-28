create sequence person_user_id_seq;

alter sequence person_user_id_seq owner to postgres;

alter sequence person_user_id_seq owned by "Person".user_id;

