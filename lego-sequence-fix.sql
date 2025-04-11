SELECT pg_catalog.setval('public.lego_colors_id_seq', (SELECT MAX(id) FROM public.lego_colors));
SELECT pg_catalog.setval('public.lego_inventories_id_seq', (SELECT MAX(id) FROM public.lego_inventories));
SELECT pg_catalog.setval('public.lego_part_categories_id_seq', (SELECT MAX(id) FROM public.lego_part_categories));
SELECT pg_catalog.setval('public.lego_themes_id_seq', (SELECT MAX(id) FROM public.lego_themes));
