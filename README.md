# LibEtrian

LibEtrian is a C# library that provides (at this time) read-only parsers for various formats used in the Etrian Odyssey series, specifically the games that use the Atlus EO engine (everything from EO3 onwards).

Currently, the following files are supported with the following classes:
* floor_system.tbl
  * DungeonSystemTable
* Gather passive tables, i.e. gather_bird.tbl, gather_dog.tbl, gather_material.tbl, gather_use.tbl
  * ItemFinderPassiveTable
    * EO5, EON
* encount_group.tbl [Incomplete, only loads enemy IDs]
  * EncounterGroupTableV1
    * EO3
  * EncounterGroupTableV2
    * EO4, EOU
  * EncounterGroupTableV3
    * EO2U, EO5, EON
* enemydata.tbl
  * EnemyDataTableV1
    * EO3, EO4
  * EnemyDataTableV2
    * EOU
  * EnemyDataTableV3
    * EO2U, EO5, EON
* enemygraphic.tbl
  * EnemyGraphicTable3DS
    * EO4, EOU, EO2U, EO5, EON
* Skill tables, i.e. playerskilltable.tbl
  * SkillTable
    * All games
* SkillTreeeTable.tbl
  * SkillTreeEO4
    * EO4
* skt_[class].tbl
  * SkillTreeSktr
    * EOU, EO2U, EO5, EON
* Stat growth tables, i.e. GrowthTable.tbl, GrowthKindTable.tbl
  * StatGrowthTable
    * All games
* BMD text files [Incomplete]
  * Bmd
    * PQ, PQ2
* MBM text files
  * Mbm
    * All games
* TBL text files
  * Table
    * All games
