create sequence addon_addon_id_seq;

alter sequence addon_addon_id_seq owner to postgres;

alter sequence addon_addon_id_seq owned by "AddOn".addon_id;

