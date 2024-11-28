create sequence party_id_seq;

alter sequence party_id_seq owner to postgres;

alter sequence party_id_seq owned by "Party".id;

