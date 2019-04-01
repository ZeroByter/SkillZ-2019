using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    public class GameCaching
    {
        private Elf[] AllEnemyElves;
        private int AllEnemyElvesLength;

        private Elf[] AllMyElves;
        private int AllMyElvesLength;

        private Elf[] EnemyLivingElves;
        private int EnemyLivingElvesLength;

        private Elf[] MyLivingElves;
        private int MyLivingElvesLength;

        private List<Elf> EnemyElvesCurrentlyBuildingPortals;
        private int EnemyElvesCurrentlyBuildingPortalsCount;

        private Dictionary<Circle, List<Elf>> EnemyElvesInAreaCurrentlyBuildingPortals;
        private int EnemyElvesInAreaCurrentlyBuildingPortalsCount;

        private Dictionary<Circle, List<Elf>> EnemyElvesInAreaCurrentlyBuildingManaFountains;
        private int EnemyElvesInAreaCurrentlyBuildingManaFountainsCount;

        private List<Elf> EnemyElvesCurrentlyBuildingManaFountains;
        private int EnemyElvesCurrentlyBuildingManaFountainsCount;

        private Dictionary<Circle, List<Elf>> EnemyElvesInAreaCurrentlyManaFountains;
        private int EnemyElvesInAreaCurrentlyManaFountainsCount;

        private Portal[] MyPortals;
        private int MyPortalsLength;

        private Portal[] EnemyPortals;
        private int EnemyPortalsLength;

        private List<Portal> EnemyPortalsCurrentlySummoningLavaGiants;
        private int EnemyPortalsCurrentlySummoningLavaGiantsCount;

        private Dictionary<Circle, List<Portal>> EnemyPortalsInAreaCurrentlySummoningLavaGiants;
        private int EnemyPortalsInAreaCurrentlySummoningLavaGiantsCount;

        private List<Portal> EnemyPortalsCurrentlySummoningIceTrolls;
        private int EnemyPortalsCurrentlySummoningIceTrollsCount;

        private Dictionary<Circle, List<Portal>> EnemyPortalsInAreaCurrentlySummoningIceTrolls;
        private int EnemyPortalsInAreaCurrentlySummoningIceTrollsCount;

        private List<Portal> EnemyPortalsCurrentlySummoningTornadoes;
        private int EnemyPortalsCurrentlySummoningTornadoesCount;

        private Dictionary<Circle, List<Portal>> EnemyPortalsInAreaCurrentlySummoningTornadoes;
        private int EnemyPortalsInAreaCurrentlySummoningTornadoesCount;

        private ManaFountain[] MyManaFountains;
        private int MyManaFountainsLength;

        private ManaFountain[] EnemyManaFountains;
        private int EnemyManaFountainsLength;

        private LavaGiant[] MyLavaGiants;
        private int MyLavaGiantsLength;

        private LavaGiant[] EnemyLavaGiants;
        private int EnemyLavaGiantsLength;

        private Tornado[] MyTornados;
        private int MyTornadosLength;

        private Tornado[] EnemyTornados;
        private int EnemyTornadosLength;

        private IceTroll[] MyIceTrolls;
        private int MyIceTrollsLength;

        private IceTroll[] EnemyIceTrolls;
        private int EnemyIceTrollsLength;

        private List<Portal> MyPortalsCurrentlySummoningLavaGiants;
        private int MyPortalsCurrentlySummoningLavaGiantsCount;

        private List<Portal> MyPortalsCurrentlySummoningIceTrolls;
        private int MyPortalsCurrentlySummoningIceTrollsCount;

        private Dictionary<Circle, List<Portal>> MyPortalsInAreaCurrentlySummoningTornadoes;
        private int MyPortalsInAreaCurrentlySummoningTornadoesCount;

        private List<Elf> MyCurrentlyBuildingPortalsElves;
        private int MyCurrentlyBuildingPortalsElvesCount;

        private List<Elf> MyCurrentlyBuildingManaFountainsElves;
        private int MyCurrentlyBuildingManaFountainsElvesCount;

        private Dictionary<int, GameObject> AllGameObjects;
        private int AllGameObjectsCount;

        private List<GameObject> MyControllableGameObjects;
        private int MyControllableGameObjectsCount;

        private List<GameObject> AllEnemyObjectsThatCanAttackElf;
        private int AllEnemyObjectsThatCanAttackElfCount;

        /*Dictionary<int, Portal> AllPortals;
        int AllPortalsCount;

        Dictionary<int, ManaFountain> AllManaFountains;
        int AllManaFountainsCount;*/

        private List<Creature> MyCreatures;
        private int MyCreaturesCount;

        private List<Creature> EnemyCreatures;
        private int EnemyCreaturesCount;

        //Dictionary<int, Creature> AllCreatures;
        //int AllCreaturesCount;

        private Dictionary<int, Portal> ClosestPortalToTornado;

        private List<GameObject> AllMyGameObjects;
        private int AllMyGameObjectsCount;

        private List<GameObject> AllEnemyGameObjects;
        private int AllEnemyGameObjectsCount;

        private Dictionary<Circle, List<Portal>> MyPortalsInAreas;
        private int MyPortalsInAreasCount;

        private Dictionary<Circle, List<Portal>> EnemyPortalsInAreas;
        private int EnemyPortalsInAreasCount;

        private Dictionary<Circle, List<Elf>> MyElvesInAreas;
        private int MyElvesInAreasCount;

        private Dictionary<Circle, List<Elf>> EnemyElvesInAreas;
        private int EnemyElvesInAreasCount;

        private Dictionary<Circle, List<LavaGiant>> MyLavaGiantsInAreas;
        private int MyLavaGiantsInAreasCount;

        private Dictionary<Circle, List<LavaGiant>> EnemyLavaGiantsInAreas;
        private int EnemyLavaGiantsInAreasCount;

        private Dictionary<Circle, List<Tornado>> MyTornadoesInAreas;
        private int MyTornadoesInAreasCount;

        private Dictionary<Circle, List<Tornado>> EnemyTornadoesInAreas;
        private int EnemyTornadoesInAreasCount;

        private Dictionary<Circle, List<IceTroll>> MyIceTrollsInAreas;
        private int MyIceTrollsInAreasCount;

        private Dictionary<Circle, List<IceTroll>> EnemyIceTrollsInAreas;
        private int EnemyIceTrollsInAreasCount;

        private Dictionary<Circle, List<ManaFountain>> MyManaFountainsInAreas;
        private int MyManaFountainsInAreasCount;

        private Dictionary<Circle, List<ManaFountain>> EnemyManaFountainsInAreas;
        private int EnemyManaFountainsInAreasCount;

        #region Base getters
        public Elf[] GetAllEnemyElves()
        {
            if (AllEnemyElves == null || AllEnemyElvesLength != AllEnemyElves.Length)
            {
                AllEnemyElves = Constants.Game.GetAllEnemyElves();
                AllEnemyElvesLength = AllEnemyElves.Length;
            }

            return AllEnemyElves;
        }

        public Elf[] GetEnemyLivingElves()
        {
            if (EnemyLivingElves == null || EnemyLivingElvesLength != EnemyLivingElves.Length)
            {
                EnemyLivingElves = Constants.Game.GetEnemyLivingElves();
                EnemyLivingElvesLength = EnemyLivingElves.Length;
            }

            return EnemyLivingElves;
        }

        public Elf[] GetAllMyElves()
        {
            if (AllMyElves == null || AllMyElvesLength != AllMyElves.Length)
            {
                AllMyElves = Constants.Game.GetAllMyElves();
                AllMyElvesLength = AllMyElves.Length;
            }

            return AllMyElves;
        }

        public Elf[] GetMyLivingElves()
        {
            if (MyLivingElves == null || MyLivingElvesLength != MyLivingElves.Length)
            {
                MyLivingElves = Constants.Game.GetMyLivingElves();
                MyLivingElvesLength = MyLivingElves.Length;
            }

            return MyLivingElves;
        }

        public Portal[] GetMyPortals()
        {
            if (MyPortals == null || MyPortalsLength != MyPortals.Length)
            {
                MyPortals = Constants.Game.GetMyPortals();
                MyPortalsLength = MyPortals.Length;
            }

            return MyPortals;
        }

        public Portal[] GetEnemyPortals()
        {
            if (EnemyPortals == null || EnemyPortalsLength != EnemyPortals.Length)
            {
                EnemyPortals = Constants.Game.GetEnemyPortals();
                EnemyPortalsLength = EnemyPortals.Length;
            }

            return EnemyPortals;
        }

        public ManaFountain[] GetMyManaFountains()
        {
            if (MyManaFountains == null || MyManaFountainsLength != MyManaFountains.Length)
            {
                MyManaFountains = Constants.Game.GetMyManaFountains();
                MyManaFountainsLength = MyManaFountains.Length;
            }

            return MyManaFountains;
        }

        public ManaFountain[] GetEnemyManaFountains()
        {
            if (EnemyManaFountains == null || EnemyManaFountainsLength != EnemyManaFountains.Length)
            {
                EnemyManaFountains = Constants.Game.GetEnemyManaFountains();
                EnemyManaFountainsLength = EnemyManaFountains.Length;
            }

            return EnemyManaFountains;
        }

        public LavaGiant[] GetMyLavaGiants()
        {
            if (MyLavaGiants == null || MyLavaGiantsLength != MyLavaGiants.Length)
            {
                MyLavaGiants = Constants.Game.GetMyLavaGiants();
                MyLavaGiantsLength = MyLavaGiants.Length;
            }

            return MyLavaGiants;
        }

        public LavaGiant[] GetEnemyLavaGiants()
        {
            if (EnemyLavaGiants == null || EnemyLavaGiantsLength != EnemyLavaGiants.Length)
            {
                EnemyLavaGiants = Constants.Game.GetEnemyLavaGiants();
                EnemyLavaGiantsLength = EnemyLavaGiants.Length;
            }

            return EnemyLavaGiants;
        }

        public Tornado[] GetMyTornadoes()
        {
            if (MyTornados == null || MyTornadosLength != MyTornados.Length)
            {
                MyTornados = Constants.Game.GetMyTornadoes();
                MyTornadosLength = MyTornados.Length;
            }

            return MyTornados;
        }

        public Tornado[] GetEnemyTornadoes()
        {
            if (EnemyTornados == null || EnemyTornadosLength != EnemyTornados.Length)
            {
                EnemyTornados = Constants.Game.GetEnemyTornadoes();
                EnemyTornadosLength = EnemyTornados.Length;
            }

            return EnemyTornados;
        }

        public IceTroll[] GetMyIceTrolls()
        {
            if (MyIceTrolls == null || MyIceTrollsLength != MyIceTrolls.Length)
            {
                MyIceTrolls = Constants.Game.GetMyIceTrolls();
                MyIceTrollsLength = MyIceTrolls.Length;
            }

            return MyIceTrolls;
        }

        public IceTroll[] GetEnemyIceTrolls()
        {
            if (EnemyIceTrolls == null || EnemyIceTrollsLength != EnemyIceTrolls.Length)
            {
                EnemyIceTrolls = Constants.Game.GetEnemyIceTrolls();
                EnemyIceTrollsLength = EnemyIceTrolls.Length;
            }

            return EnemyIceTrolls;
        }
        #endregion

        #region Get combined lists of things that are already cached

        #endregion

        #region Filtered lists
        private List<Portal> GetMyCurrentlySummoningPortals(string currentlySummoning)
        {
            List<Portal> portals = new List<Portal>();

            foreach (Portal portal in GetMyPortals())
            {
                if (portal.IsSummoning && portal.CurrentlySummoning == currentlySummoning) portals.Add(portal);
            }

            return portals;
        }

        public List<Portal> GetMyCurrentlySummoningLavaGiantsPortals()
        {
            if (MyPortalsCurrentlySummoningLavaGiants == null || MyPortalsCurrentlySummoningLavaGiantsCount != MyPortalsCurrentlySummoningLavaGiants.Count)
            {
                MyPortalsCurrentlySummoningLavaGiants = GetMyCurrentlySummoningPortals("LavaGiant");
                MyPortalsCurrentlySummoningLavaGiantsCount = MyPortalsCurrentlySummoningLavaGiants.Count;
            }

            return MyPortalsCurrentlySummoningLavaGiants;
        }

        public List<Portal> GetMyCurrentlySummoningIceTrollsPortals()
        {
            if (MyPortalsCurrentlySummoningIceTrolls == null || MyPortalsCurrentlySummoningIceTrollsCount != MyPortalsCurrentlySummoningIceTrolls.Count)
            {
                MyPortalsCurrentlySummoningIceTrolls = GetMyCurrentlySummoningPortals("IceTroll");
                MyPortalsCurrentlySummoningIceTrollsCount = MyPortalsCurrentlySummoningIceTrolls.Count;
            }

            return MyPortalsCurrentlySummoningIceTrolls;
        }

        private List<Elf> GetMyCurrentlyBuildingPortalsElves(string currentlyBuilding)
        {
            List<Elf> elves = new List<Elf>();

            foreach (Elf elf in GetMyLivingElves())
            {
                if (elf.IsBuilding && elf.CurrentlyBuilding == currentlyBuilding) elves.Add(elf);
            }

            return elves;
        }

        public List<Elf> GetMyCurrentlyBuildingPortalsElves()
        {
            if (MyCurrentlyBuildingPortalsElves == null || MyCurrentlyBuildingPortalsElvesCount != MyCurrentlyBuildingPortalsElves.Count)
            {
                MyCurrentlyBuildingPortalsElves = GetMyCurrentlyBuildingPortalsElves("Portal");
                MyCurrentlyBuildingPortalsElvesCount = MyCurrentlyBuildingPortalsElves.Count;
            }

            return MyCurrentlyBuildingPortalsElves;
        }

        public List<Elf> GetMyCurrentlyBuildingManaFountainsElves()
        {
            if (MyCurrentlyBuildingManaFountainsElves == null || MyCurrentlyBuildingManaFountainsElvesCount != MyCurrentlyBuildingManaFountainsElves.Count)
            {
                MyCurrentlyBuildingManaFountainsElves = GetMyCurrentlyBuildingPortalsElves("ManaFountain");
                MyCurrentlyBuildingManaFountainsElvesCount = MyCurrentlyBuildingManaFountainsElves.Count;
            }

            return MyCurrentlyBuildingManaFountainsElves;
        }

        private void AddListToDictionary(Dictionary<int, GameObject> dictionary, GameObject[] array)
        {
            foreach (GameObject gameObject in array)
            {
                dictionary[gameObject.UniqueId] = gameObject;
            }
        }

        /*public Dictionary<int, Portal> GetAllPortals()
        {
            if (AllPortals == null || AllPortalsCount != AllPortals.Count)
            {
                AllPortals = new Dictionary<int, Portal>();

                foreach (var gameObject in GetMyPortals()) AllPortals.Add(gameObject.UniqueId, gameObject);
                foreach (var gameObject in GetEnemyPortals()) AllPortals.Add(gameObject.UniqueId, gameObject);

                AllPortalsCount = AllPortals.Count;
            }

            return AllPortals;
        }

        public Dictionary<int, ManaFountain> GetAllManaFountains()
        {
            if (AllManaFountains == null || AllManaFountainsCount != AllManaFountains.Count)
            {
                AllManaFountains = new Dictionary<int, ManaFountain>();

                foreach (var gameObject in GetMyManaFountains()) AllManaFountains.Add(gameObject.UniqueId, gameObject);
                foreach (var gameObject in GetEnemyManaFountains()) AllManaFountains.Add(gameObject.UniqueId, gameObject);

                AllManaFountainsCount = AllManaFountains.Count;
            }

            return AllManaFountains;
        }*/

        public List<Creature> GetMyCreatures()
        {
            if (MyCreatures == null || MyCreaturesCount != MyCreatures.Count)
            {
                MyCreatures = new List<Creature>();

                foreach (var gameObject in GetMyLavaGiants()) MyCreatures.Add(gameObject);
                foreach (var gameObject in GetMyIceTrolls()) MyCreatures.Add(gameObject);
                foreach (var gameObject in GetMyTornadoes()) MyCreatures.Add(gameObject);

                MyCreaturesCount = MyCreatures.Count;
            }

            return MyCreatures;
        }

        public List<Creature> GetEnemyCreatures()
        {
            if (EnemyCreatures == null || EnemyCreaturesCount != EnemyCreatures.Count)
            {
                EnemyCreatures = new List<Creature>();

                foreach (var gameObject in GetEnemyLavaGiants()) EnemyCreatures.Add(gameObject);
                foreach (var gameObject in GetEnemyIceTrolls()) EnemyCreatures.Add(gameObject);
                foreach (var gameObject in GetEnemyTornadoes()) EnemyCreatures.Add(gameObject);

                EnemyCreaturesCount = EnemyCreatures.Count;
            }

            return EnemyCreatures;
        }

        /*public Dictionary<int, Creature> GetAllCreatures()
        {
            if (AllCreatures == null || AllCreaturesCount != AllCreatures.Count)
            {
                AllCreatures = new Dictionary<int, Creature>();

                foreach (var gameObject in GetMyLavaGiants()) AllCreatures.Add(gameObject.UniqueId, gameObject);
                foreach (var gameObject in GetEnemyLavaGiants()) AllCreatures.Add(gameObject.UniqueId, gameObject);
                foreach (var gameObject in GetMyIceTrolls()) AllCreatures.Add(gameObject.UniqueId, gameObject);
                foreach (var gameObject in GetEnemyIceTrolls()) AllCreatures.Add(gameObject.UniqueId, gameObject);

                AllCreaturesCount = AllCreatures.Count;
            }

            return AllCreatures;
        }*/

        public Dictionary<int, GameObject> GetAllGameObjects()
        {
            if (AllGameObjects == null || AllGameObjectsCount != AllGameObjects.Count)
            {
                AllGameObjects = new Dictionary<int, GameObject>();

                foreach (var pair in GetAllMyGameObjects()) AllGameObjects.Add(pair.UniqueId, pair);
                foreach (var pair in GetAllEnemyGameObjects()) AllGameObjects.Add(pair.UniqueId, pair);

                AllGameObjectsCount = AllGameObjects.Count;
            }

            return AllGameObjects;
        }

        public GameObject GetGameObjectByUniqueId(int uniqueId)
        {
            GameObject gameObject = null;

            GetAllGameObjects().TryGetValue(uniqueId, out gameObject);

            return gameObject;
        }

        public List<GameObject> GetMyControllableGameObjects()
        {
            if (MyControllableGameObjects == null || MyControllableGameObjectsCount != MyControllableGameObjects.Count)
            {
                MyControllableGameObjects = new List<GameObject>();

                foreach (var gameObject in GetMyPortals()) MyControllableGameObjects.Add(gameObject);
                foreach (var gameObject in GetMyLivingElves()) MyControllableGameObjects.Add(gameObject);

                MyControllableGameObjectsCount = MyControllableGameObjects.Count;
            }

            return MyControllableGameObjects;
        }

        public List<GameObject> GetAllEnemyObjectsThatCanAttackElf()
        {
            if (AllEnemyObjectsThatCanAttackElf == null || AllEnemyObjectsThatCanAttackElfCount != AllEnemyObjectsThatCanAttackElf.Count)
            {
                AllEnemyObjectsThatCanAttackElf = new List<GameObject>();

                AllEnemyObjectsThatCanAttackElf.AddRange(GetEnemyLivingElves());
                AllEnemyObjectsThatCanAttackElf.AddRange(GetEnemyIceTrolls());

                AllEnemyObjectsThatCanAttackElfCount = AllEnemyObjectsThatCanAttackElf.Count;
            }

            return AllEnemyObjectsThatCanAttackElf;
        }

        public List<GameObject> GetAllMyGameObjects()
        {
            if (AllMyGameObjects == null || AllMyGameObjectsCount != AllMyGameObjects.Count)
            {
                AllMyGameObjects = new List<GameObject>();

                AllMyGameObjects.Add(Constants.Game.GetMyCastle());
                AllMyGameObjects.AddRange(GetMyPortals());
                AllMyGameObjects.AddRange(GetMyCreatures());
                AllMyGameObjects.AddRange(GetMyLivingElves());
                AllMyGameObjects.AddRange(GetMyManaFountains());

                AllMyGameObjectsCount = AllMyGameObjects.Count;
            }

            return AllMyGameObjects;
        }

        public List<GameObject> GetAllEnemyGameObjects()
        {
            if (AllEnemyGameObjects == null || AllEnemyGameObjectsCount != AllEnemyGameObjects.Count)
            {
                AllEnemyGameObjects = new List<GameObject>();

                AllEnemyGameObjects.Add(Constants.Game.GetVolcano());
                AllEnemyGameObjects.Add(Constants.Game.GetEnemyCastle());
                AllEnemyGameObjects.AddRange(GetEnemyPortals());
                AllEnemyGameObjects.AddRange(GetEnemyCreatures());
                AllEnemyGameObjects.AddRange(GetEnemyLivingElves());
                AllEnemyGameObjects.AddRange(GetEnemyManaFountains());

                AllEnemyGameObjectsCount = AllEnemyGameObjects.Count;
            }

            return AllEnemyGameObjects;
        }
        #endregion

        #region Closest Portal To Tornado
        public Portal GetMyClosestPortalToEnemyTornado(Tornado enemyTornado)
        {
            Portal portal = null;

            if(ClosestPortalToTornado == null)
            {
                ClosestPortalToTornado = new Dictionary<int, Portal>();

                portal = (Portal)GetMyPortals().GetClosest(enemyTornado);
                ClosestPortalToTornado[enemyTornado.UniqueId] = portal;
            }
            else
            {
                if(!ClosestPortalToTornado.TryGetValue(enemyTornado.UniqueId, out portal))
                {
                    portal = (Portal)GetMyPortals().GetClosest(enemyTornado);
                    ClosestPortalToTornado[enemyTornado.UniqueId] = portal;
                }
            }

            return portal;
        }
        #endregion

        #region Functions to get stuff that is summoning/building other stuff
        private List<Portal> CalculateEnemyPortalsCurrentlySummoning(string type, Circle area = null)
        {
            List<Portal> portals = new List<Portal>();

            if (area == null)
            {
                foreach (Portal portal in GetEnemyPortals())
                {
                    if (portal.IsSummoning && portal.CurrentlySummoning == type) portals.Add(portal);
                }
            }
            else
            {
                foreach (Portal portal in GetEnemyPortalsInArea(area))
                {
                    if (portal.IsSummoning && portal.CurrentlySummoning == type) portals.Add(portal);
                }
            }

            return portals;
        }

        public List<Portal> GetEnemyPortalsCurrentlySummoningLavaGiants()
        {
            if (EnemyPortalsCurrentlySummoningLavaGiants == null || EnemyPortalsCurrentlySummoningLavaGiantsCount != EnemyPortalsCurrentlySummoningLavaGiants.Count)
            {
                EnemyPortalsCurrentlySummoningLavaGiants = CalculateEnemyPortalsCurrentlySummoning("LavaGiant");
                EnemyPortalsCurrentlySummoningLavaGiantsCount = EnemyPortalsCurrentlySummoningLavaGiants.Count;
            }

            return EnemyPortalsCurrentlySummoningLavaGiants;
        }

        public List<Portal> GetEnemyPortalsInAreaCurrentlySummoningLavaGiants(Circle area)
        {
            List<Portal> enemyPortalsInAreaCurrentlySummoning = null;

            if (EnemyPortalsInAreaCurrentlySummoningLavaGiants == null || EnemyPortalsInAreaCurrentlySummoningLavaGiantsCount != EnemyPortalsInAreaCurrentlySummoningLavaGiants.Count)
            {
                EnemyPortalsInAreaCurrentlySummoningLavaGiants = new Dictionary<Circle, List<Portal>>();

                enemyPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("LavaGiant", area);
                EnemyPortalsInAreaCurrentlySummoningLavaGiants[area] = enemyPortalsInAreaCurrentlySummoning;
                EnemyPortalsInAreaCurrentlySummoningLavaGiantsCount = EnemyPortalsInAreaCurrentlySummoningLavaGiants.Count;
            }
            else
            {
                if (!EnemyPortalsInAreaCurrentlySummoningLavaGiants.TryGetValue(area, out enemyPortalsInAreaCurrentlySummoning))
                {
                    enemyPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("LavaGiant", area);
                    EnemyPortalsInAreaCurrentlySummoningLavaGiants[area] = enemyPortalsInAreaCurrentlySummoning;
                    EnemyPortalsInAreaCurrentlySummoningLavaGiantsCount = EnemyPortalsInAreaCurrentlySummoningLavaGiants.Count;
                }
            }

            return enemyPortalsInAreaCurrentlySummoning;
        }

        public List<Portal> GetEnemyPortalsCurrentlySummoningIceTrolls()
        {
            if (EnemyPortalsCurrentlySummoningIceTrolls == null || EnemyPortalsCurrentlySummoningIceTrollsCount != EnemyPortalsCurrentlySummoningIceTrolls.Count)
            {
                EnemyPortalsCurrentlySummoningIceTrolls = CalculateEnemyPortalsCurrentlySummoning("IceTroll");
                EnemyPortalsCurrentlySummoningIceTrollsCount = EnemyPortalsCurrentlySummoningIceTrolls.Count;
            }

            return EnemyPortalsCurrentlySummoningIceTrolls;
        }

        public List<Portal> GetEnemyPortalsInAreaCurrentlySummoningIceTrolls(Circle area)
        {
            List<Portal> enemyPortalsInAreaCurrentlySummoning = null;

            if (EnemyPortalsInAreaCurrentlySummoningIceTrolls == null || EnemyPortalsInAreaCurrentlySummoningIceTrollsCount != EnemyPortalsInAreaCurrentlySummoningIceTrolls.Count)
            {
                EnemyPortalsInAreaCurrentlySummoningIceTrolls = new Dictionary<Circle, List<Portal>>();

                enemyPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("IceTroll", area);
                EnemyPortalsInAreaCurrentlySummoningIceTrolls[area] = enemyPortalsInAreaCurrentlySummoning;
                EnemyPortalsInAreaCurrentlySummoningIceTrollsCount = EnemyPortalsInAreaCurrentlySummoningIceTrolls.Count;
            }
            else
            {
                if (!EnemyPortalsInAreaCurrentlySummoningIceTrolls.TryGetValue(area, out enemyPortalsInAreaCurrentlySummoning))
                {
                    enemyPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("IceTroll", area);
                    EnemyPortalsInAreaCurrentlySummoningIceTrolls[area] = enemyPortalsInAreaCurrentlySummoning;
                    EnemyPortalsInAreaCurrentlySummoningIceTrollsCount = EnemyPortalsInAreaCurrentlySummoningIceTrolls.Count;
                }
            }

            return enemyPortalsInAreaCurrentlySummoning;
        }

        public List<Portal> GetEnemyPortalsCurrentlySummoningTornadoes()
        {
            if (EnemyPortalsCurrentlySummoningTornadoes == null || EnemyPortalsCurrentlySummoningTornadoesCount != EnemyPortalsCurrentlySummoningTornadoes.Count)
            {
                EnemyPortalsCurrentlySummoningTornadoes = CalculateEnemyPortalsCurrentlySummoning("Tornado");
                EnemyPortalsCurrentlySummoningTornadoesCount = EnemyPortalsCurrentlySummoningTornadoes.Count;
            }

            return EnemyPortalsCurrentlySummoningTornadoes;
        }

        public List<Portal> GetEnemyPortalsInAreaCurrentlySummoningTornadoes(Circle area)
        {
            List<Portal> enemyPortalsInAreaCurrentlySummoning = null;

            if (EnemyPortalsInAreaCurrentlySummoningTornadoes == null || EnemyPortalsInAreaCurrentlySummoningTornadoesCount != EnemyPortalsInAreaCurrentlySummoningTornadoes.Count)
            {
                EnemyPortalsInAreaCurrentlySummoningTornadoes = new Dictionary<Circle, List<Portal>>();

                enemyPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("Tornado", area);
                EnemyPortalsInAreaCurrentlySummoningTornadoes[area] = enemyPortalsInAreaCurrentlySummoning;
                EnemyPortalsInAreaCurrentlySummoningTornadoesCount = EnemyPortalsInAreaCurrentlySummoningTornadoes.Count;
            }
            else
            {
                if (!EnemyPortalsInAreaCurrentlySummoningTornadoes.TryGetValue(area, out enemyPortalsInAreaCurrentlySummoning))
                {
                    enemyPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("Tornado", area);
                    EnemyPortalsInAreaCurrentlySummoningTornadoes[area] = enemyPortalsInAreaCurrentlySummoning;
                    EnemyPortalsInAreaCurrentlySummoningTornadoesCount = EnemyPortalsInAreaCurrentlySummoningTornadoes.Count;
                }
            }

            return enemyPortalsInAreaCurrentlySummoning;
        }

        public List<Portal> GetMyPortalsInAreaCurrentlySummoningTornadoes(Circle area)
        {
            List<Portal> myPortalsInAreaCurrentlySummoning = null;

            if (MyPortalsInAreaCurrentlySummoningTornadoes == null || MyPortalsInAreaCurrentlySummoningTornadoesCount != MyPortalsInAreaCurrentlySummoningTornadoes.Count)
            {
                MyPortalsInAreaCurrentlySummoningTornadoes = new Dictionary<Circle, List<Portal>>();

                myPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("Tornado", area);
                MyPortalsInAreaCurrentlySummoningTornadoes[area] = myPortalsInAreaCurrentlySummoning;
                MyPortalsInAreaCurrentlySummoningTornadoesCount = MyPortalsInAreaCurrentlySummoningTornadoes.Count;
            }
            else
            {
                if (!MyPortalsInAreaCurrentlySummoningTornadoes.TryGetValue(area, out myPortalsInAreaCurrentlySummoning))
                {
                    myPortalsInAreaCurrentlySummoning = CalculateEnemyPortalsCurrentlySummoning("Tornado", area);
                    MyPortalsInAreaCurrentlySummoningTornadoes[area] = myPortalsInAreaCurrentlySummoning;
                    MyPortalsInAreaCurrentlySummoningTornadoesCount = MyPortalsInAreaCurrentlySummoningTornadoes.Count;
                }
            }

            return myPortalsInAreaCurrentlySummoning;
        }

        private List<Elf> CalculateMyElvesCurrentlyBuilding(string type)
        {
            List<Elf> elves = new List<Elf>();

            foreach (Elf elf in GetMyLivingElves())
            {
                if (elf.IsBuilding && elf.CurrentlyBuilding == type) elves.Add(elf);
            }

            return elves;
        }

        private List<Elf> CalculateEnemyElvesCurrentlyBuilding(string type, Circle area = null)
        {
            List<Elf> elves = new List<Elf>();

            if (area == null)
            {
                foreach (Elf elf in GetEnemyLivingElves())
                {
                    if (elf.IsBuilding && elf.CurrentlyBuilding == type) elves.Add(elf);
                }
            }
            else
            {
                foreach (Elf elf in GetEnemyElvesInArea(area))
                {
                    if (elf.IsBuilding && elf.CurrentlyBuilding == type) elves.Add(elf);
                }
            }

            return elves;
        }

        public List<Elf> GetEnemyElvesCurrentlyBuildingManaFountains()
        {
            if (EnemyElvesCurrentlyBuildingManaFountains == null || EnemyElvesCurrentlyBuildingManaFountainsCount != EnemyElvesCurrentlyBuildingManaFountains.Count)
            {
                EnemyElvesCurrentlyBuildingManaFountains = CalculateEnemyElvesCurrentlyBuilding("ManaFountain");
                EnemyElvesCurrentlyBuildingManaFountainsCount = EnemyElvesCurrentlyBuildingManaFountains.Count;
            }

            return EnemyElvesCurrentlyBuildingManaFountains;
        }

        public List<Elf> GetEnemyElvesInAreaCurrentlyBuildingPortals(Circle area)
        {
            List<Elf> enemyElvesinAreaCurrentlyBuildingPortals = null;

            if (EnemyElvesInAreaCurrentlyBuildingPortals == null || EnemyElvesInAreaCurrentlyBuildingPortalsCount != EnemyElvesInAreaCurrentlyBuildingPortals.Count)
            {
                EnemyElvesInAreaCurrentlyBuildingPortals = new Dictionary<Circle, List<Elf>>();

                enemyElvesinAreaCurrentlyBuildingPortals = CalculateEnemyElvesCurrentlyBuilding("Portal", area);
                EnemyElvesInAreaCurrentlyBuildingPortals[area] = enemyElvesinAreaCurrentlyBuildingPortals;
                EnemyElvesInAreaCurrentlyBuildingPortalsCount = EnemyElvesInAreaCurrentlyBuildingPortals.Count;
            }
            else
            {
                if (!EnemyElvesInAreaCurrentlyBuildingPortals.TryGetValue(area, out enemyElvesinAreaCurrentlyBuildingPortals))
                {
                    enemyElvesinAreaCurrentlyBuildingPortals = CalculateEnemyElvesCurrentlyBuilding("Portal", area);
                    EnemyElvesInAreaCurrentlyBuildingPortals[area] = enemyElvesinAreaCurrentlyBuildingPortals;
                    EnemyElvesInAreaCurrentlyBuildingPortalsCount = EnemyElvesInAreaCurrentlyBuildingPortals.Count;
                }
            }

            return enemyElvesinAreaCurrentlyBuildingPortals;
        }

        public List<Elf> GetEnemyElvesInAreaCurrentlyBuildingManaFountains(Circle area)
        {
            List<Elf> enemyElvesinAreaCurrentlyBuildingManaFountains = null;

            if (EnemyElvesInAreaCurrentlyBuildingManaFountains == null || EnemyElvesInAreaCurrentlyBuildingManaFountainsCount != EnemyElvesInAreaCurrentlyBuildingManaFountains.Count)
            {
                EnemyElvesInAreaCurrentlyBuildingManaFountains = new Dictionary<Circle, List<Elf>>();

                enemyElvesinAreaCurrentlyBuildingManaFountains = CalculateEnemyElvesCurrentlyBuilding("ManaFountain", area);
                EnemyElvesInAreaCurrentlyBuildingManaFountains[area] = enemyElvesinAreaCurrentlyBuildingManaFountains;
                EnemyElvesInAreaCurrentlyBuildingManaFountainsCount = EnemyElvesInAreaCurrentlyBuildingManaFountains.Count;
            }
            else
            {
                if (!EnemyElvesInAreaCurrentlyBuildingManaFountains.TryGetValue(area, out enemyElvesinAreaCurrentlyBuildingManaFountains))
                {
                    enemyElvesinAreaCurrentlyBuildingManaFountains = CalculateEnemyElvesCurrentlyBuilding("ManaFountain", area);
                    EnemyElvesInAreaCurrentlyBuildingManaFountains[area] = enemyElvesinAreaCurrentlyBuildingManaFountains;
                    EnemyElvesInAreaCurrentlyBuildingManaFountainsCount = EnemyElvesInAreaCurrentlyBuildingManaFountains.Count;
                }
            }

            return enemyElvesinAreaCurrentlyBuildingManaFountains;
        }
        #endregion

        #region Functions to get specific stuff in areas
        private List<Portal> CalculateMyPortalsInArea(Circle area)
        {
            List<Portal> portals = new List<Portal>();

            foreach (Portal portal in GetMyPortals())
            {
                if (area.IsLocationInside(portal)) portals.Add(portal);
            }

            return portals;
        }

        public List<Portal> GetMyPortalsInArea(Circle area)
        {
            List<Portal> myPortalsInArea = null;

            if (MyPortalsInAreas == null || MyPortalsInAreasCount != MyPortalsInAreas.Count)
            {
                MyPortalsInAreas = new Dictionary<Circle, List<Portal>>();

                myPortalsInArea = CalculateMyPortalsInArea(area);
                MyPortalsInAreas[area] = myPortalsInArea;
                MyPortalsInAreasCount = MyPortalsInAreas.Count;
            }
            else
            {
                if (!MyPortalsInAreas.TryGetValue(area, out myPortalsInArea))
                {
                    myPortalsInArea = CalculateMyPortalsInArea(area);
                    MyPortalsInAreas[area] = myPortalsInArea;
                    MyPortalsInAreasCount = MyPortalsInAreas.Count;
                }
            }

            return myPortalsInArea;
        }

        private List<Portal> CalculateEnemyPortalsInArea(Circle area)
        {
            List<Portal> portals = new List<Portal>();

            foreach (Portal portal in GetEnemyPortals())
            {
                if (area.IsLocationInside(portal)) portals.Add(portal);
            }

            return portals;
        }

        public List<Portal> GetEnemyPortalsInArea(Circle area)
        {
            List<Portal> enemyPortalsInArea = null;

            if (EnemyPortalsInAreas == null || EnemyPortalsInAreasCount != EnemyPortalsInAreas.Count)
            {
                EnemyPortalsInAreas = new Dictionary<Circle, List<Portal>>();

                enemyPortalsInArea = CalculateEnemyPortalsInArea(area);
                EnemyPortalsInAreas[area] = enemyPortalsInArea;
                EnemyPortalsInAreasCount = EnemyPortalsInAreas.Count;
            }
            else
            {
                if (!EnemyPortalsInAreas.TryGetValue(area, out enemyPortalsInArea))
                {
                    enemyPortalsInArea = CalculateEnemyPortalsInArea(area);
                    EnemyPortalsInAreas[area] = enemyPortalsInArea;
                    EnemyPortalsInAreasCount = EnemyPortalsInAreas.Count;
                }
            }

            return enemyPortalsInArea;
        }

        private List<Elf> CalculateMyElvesInArea(Circle area)
        {
            List<Elf> elves = new List<Elf>();

            foreach (Elf elf in GetMyLivingElves())
            {
                if (area.IsLocationInside(elf)) elves.Add(elf);
            }

            return elves;
        }

        public List<Elf> GetMyElvesInArea(Circle area)
        {
            List<Elf> myElvesInArea = null;

            if (MyElvesInAreas == null || MyElvesInAreasCount != MyElvesInAreas.Count)
            {
                MyElvesInAreas = new Dictionary<Circle, List<Elf>>();

                myElvesInArea = CalculateMyElvesInArea(area);
                MyElvesInAreas[area] = myElvesInArea;
                MyElvesInAreasCount = MyElvesInAreas.Count;
            }
            else
            {
                if (!MyElvesInAreas.TryGetValue(area, out myElvesInArea))
                {
                    myElvesInArea = CalculateMyElvesInArea(area);
                    MyElvesInAreas[area] = myElvesInArea;
                    MyElvesInAreasCount = MyElvesInAreas.Count;
                }
            }

            return myElvesInArea;
        }

        private List<Elf> CalculateEnemyElvesInArea(Circle area)
        {
            List<Elf> elves = new List<Elf>();

            foreach (Elf elf in GetEnemyLivingElves())
            {
                if (area.IsLocationInside(elf)) elves.Add(elf);
            }

            return elves;
        }

        public List<Elf> GetEnemyElvesInArea(Circle area)
        {
            List<Elf> enemyElvesInArea = null;

            if (EnemyElvesInAreas == null || EnemyElvesInAreasCount != EnemyElvesInAreas.Count)
            {
                EnemyElvesInAreas = new Dictionary<Circle, List<Elf>>();

                enemyElvesInArea = CalculateEnemyElvesInArea(area);
                EnemyElvesInAreas[area] = enemyElvesInArea;
                EnemyElvesInAreasCount = EnemyElvesInAreas.Count;
            }
            else
            {
                if (!EnemyElvesInAreas.TryGetValue(area, out enemyElvesInArea))
                {
                    enemyElvesInArea = CalculateEnemyElvesInArea(area);
                    EnemyElvesInAreas[area] = enemyElvesInArea;
                    EnemyElvesInAreasCount = EnemyElvesInAreas.Count;
                }
            }

            return enemyElvesInArea;
        }

        private List<LavaGiant> CalculateMyLavaGiantsInArea(Circle area)
        {
            List<LavaGiant> LavaGiants = new List<LavaGiant>();

            foreach (LavaGiant LavaGiant in GetMyLavaGiants())
            {
                if (area.IsLocationInside(LavaGiant)) LavaGiants.Add(LavaGiant);
            }

            return LavaGiants;
        }

        public List<LavaGiant> GetMyLavaGiantsInArea(Circle area)
        {
            List<LavaGiant> myLavaGiantsInArea = null;

            if (MyLavaGiantsInAreas == null || MyLavaGiantsInAreasCount != MyLavaGiantsInAreas.Count)
            {
                MyLavaGiantsInAreas = new Dictionary<Circle, List<LavaGiant>>();

                myLavaGiantsInArea = CalculateMyLavaGiantsInArea(area);
                MyLavaGiantsInAreas[area] = myLavaGiantsInArea;
                MyLavaGiantsInAreasCount = MyLavaGiantsInAreas.Count;
            }
            else
            {
                if (!MyLavaGiantsInAreas.TryGetValue(area, out myLavaGiantsInArea))
                {
                    myLavaGiantsInArea = CalculateMyLavaGiantsInArea(area);
                    MyLavaGiantsInAreas[area] = myLavaGiantsInArea;
                    MyLavaGiantsInAreasCount = MyLavaGiantsInAreas.Count;
                }
            }

            return myLavaGiantsInArea;
        }

        private List<LavaGiant> CalculateEnemyLavaGiantsInArea(Circle area)
        {
            List<LavaGiant> LavaGiants = new List<LavaGiant>();

            foreach (LavaGiant LavaGiant in GetEnemyLavaGiants())
            {
                if (area.IsLocationInside(LavaGiant)) LavaGiants.Add(LavaGiant);
            }

            return LavaGiants;
        }

        public List<LavaGiant> GetEnemyLavaGiantsInArea(Circle area)
        {
            List<LavaGiant> EnemyLavaGiantsInArea = null;

            if (EnemyLavaGiantsInAreas == null || EnemyLavaGiantsInAreasCount != EnemyLavaGiantsInAreas.Count)
            {
                EnemyLavaGiantsInAreas = new Dictionary<Circle, List<LavaGiant>>();

                EnemyLavaGiantsInArea = CalculateEnemyLavaGiantsInArea(area);
                EnemyLavaGiantsInAreas[area] = EnemyLavaGiantsInArea;
                EnemyLavaGiantsInAreasCount = EnemyLavaGiantsInAreas.Count;
            }
            else
            {
                if (!EnemyLavaGiantsInAreas.TryGetValue(area, out EnemyLavaGiantsInArea))
                {
                    EnemyLavaGiantsInArea = CalculateEnemyLavaGiantsInArea(area);
                    EnemyLavaGiantsInAreas[area] = EnemyLavaGiantsInArea;
                    EnemyLavaGiantsInAreasCount = EnemyLavaGiantsInAreas.Count;
                }
            }

            return EnemyLavaGiantsInArea;
        }

        private List<Tornado> CalculateMyTornadoesInArea(Circle area)
        {
            List<Tornado> Tornadoes = new List<Tornado>();

            foreach (Tornado Tornado in GetMyTornadoes())
            {
                if (area.IsLocationInside(Tornado)) Tornadoes.Add(Tornado);
            }

            return Tornadoes;
        }

        public List<Tornado> GetMyTornadoesInArea(Circle area)
        {
            List<Tornado> myTornadoesInArea = null;

            if (MyTornadoesInAreas == null || MyTornadoesInAreasCount != MyTornadoesInAreas.Count)
            {
                MyTornadoesInAreas = new Dictionary<Circle, List<Tornado>>();

                myTornadoesInArea = CalculateMyTornadoesInArea(area);
                MyTornadoesInAreas[area] = myTornadoesInArea;
                MyTornadoesInAreasCount = MyTornadoesInAreas.Count;
            }
            else
            {
                if (!MyTornadoesInAreas.TryGetValue(area, out myTornadoesInArea))
                {
                    myTornadoesInArea = CalculateMyTornadoesInArea(area);
                    MyTornadoesInAreas[area] = myTornadoesInArea;
                    MyTornadoesInAreasCount = MyTornadoesInAreas.Count;
                }
            }

            return myTornadoesInArea;
        }

        private List<Tornado> CalculateEnemyTornadoesInArea(Circle area)
        {
            List<Tornado> Tornadoes = new List<Tornado>();

            foreach (Tornado Tornado in GetEnemyTornadoes())
            {
                if (area.IsLocationInside(Tornado)) Tornadoes.Add(Tornado);
            }

            return Tornadoes;
        }

        public List<Tornado> GetEnemyTornadoesInArea(Circle area)
        {
            List<Tornado> EnemyTornadoesInArea = null;

            if (EnemyTornadoesInAreas == null || EnemyTornadoesInAreasCount != EnemyTornadoesInAreas.Count)
            {
                EnemyTornadoesInAreas = new Dictionary<Circle, List<Tornado>>();

                EnemyTornadoesInArea = CalculateEnemyTornadoesInArea(area);
                EnemyTornadoesInAreas[area] = EnemyTornadoesInArea;
                EnemyTornadoesInAreasCount = EnemyTornadoesInAreas.Count;
            }
            else
            {
                if (!EnemyTornadoesInAreas.TryGetValue(area, out EnemyTornadoesInArea))
                {
                    EnemyTornadoesInArea = CalculateEnemyTornadoesInArea(area);
                    EnemyTornadoesInAreas[area] = EnemyTornadoesInArea;
                    EnemyTornadoesInAreasCount = EnemyTornadoesInAreas.Count;
                }
            }

            return EnemyTornadoesInArea;
        }

        private List<IceTroll> CalculateMyIceTrollsInArea(Circle area)
        {
            List<IceTroll> IceTrolls = new List<IceTroll>();

            foreach (IceTroll IceTroll in GetMyIceTrolls())
            {
                if (area.IsLocationInside(IceTroll)) IceTrolls.Add(IceTroll);
            }

            return IceTrolls;
        }

        public List<IceTroll> GetMyIceTrollsInArea(Circle area)
        {
            List<IceTroll> MyIceTrollsInArea = null;

            if (MyIceTrollsInAreas == null || MyIceTrollsInAreasCount != MyIceTrollsInAreas.Count)
            {
                MyIceTrollsInAreas = new Dictionary<Circle, List<IceTroll>>();

                MyIceTrollsInArea = CalculateMyIceTrollsInArea(area);
                MyIceTrollsInAreas[area] = MyIceTrollsInArea;
                MyIceTrollsInAreasCount = MyIceTrollsInAreas.Count;
            }
            else
            {
                if (!MyIceTrollsInAreas.TryGetValue(area, out MyIceTrollsInArea))
                {
                    MyIceTrollsInArea = CalculateMyIceTrollsInArea(area);
                    MyIceTrollsInAreas[area] = MyIceTrollsInArea;
                    MyIceTrollsInAreasCount = MyIceTrollsInAreas.Count;
                }
            }

            return MyIceTrollsInArea;
        }

        private List<IceTroll> CalculateEnemyIceTrollsInArea(Circle area)
        {
            List<IceTroll> IceTrolls = new List<IceTroll>();

            foreach (IceTroll IceTroll in GetEnemyIceTrolls())
            {
                if (area.IsLocationInside(IceTroll)) IceTrolls.Add(IceTroll);
            }

            return IceTrolls;
        }

        public List<IceTroll> GetEnemyIceTrollsInArea(Circle area)
        {
            List<IceTroll> EnemyIceTrollsInArea = null;

            if (EnemyIceTrollsInAreas == null || EnemyIceTrollsInAreasCount != EnemyIceTrollsInAreas.Count)
            {
                EnemyIceTrollsInAreas = new Dictionary<Circle, List<IceTroll>>();

                EnemyIceTrollsInArea = CalculateEnemyIceTrollsInArea(area);
                EnemyIceTrollsInAreas[area] = EnemyIceTrollsInArea;
                EnemyIceTrollsInAreasCount = EnemyIceTrollsInAreas.Count;
            }
            else
            {
                if (!EnemyIceTrollsInAreas.TryGetValue(area, out EnemyIceTrollsInArea))
                {
                    EnemyIceTrollsInArea = CalculateEnemyIceTrollsInArea(area);
                    EnemyIceTrollsInAreas[area] = EnemyIceTrollsInArea;
                    EnemyIceTrollsInAreasCount = EnemyIceTrollsInAreas.Count;
                }
            }

            return EnemyIceTrollsInArea;
        }

        private List<ManaFountain> CalculateMyManaFountainsInArea(Circle area)
        {
            List<ManaFountain> manaFountains = new List<ManaFountain>();

            foreach (ManaFountain portal in GetMyManaFountains())
            {
                if (area.IsLocationInside(portal)) manaFountains.Add(portal);
            }

            return manaFountains;
        }

        private List<ManaFountain> CalculateEnemyManaFountainsInArea(Circle area)
        {
            List<ManaFountain> manaFountains = new List<ManaFountain>();

            foreach (ManaFountain manaFountain in GetEnemyManaFountains())
            {
                if (area.IsLocationInside(manaFountain)) manaFountains.Add(manaFountain);
            }

            return manaFountains;
        }

        public List<ManaFountain> GetMyManaFountainsInArea(Circle area)
        {
            List<ManaFountain> myManaFountainsInArea = null;

            if (MyManaFountainsInAreas == null || MyManaFountainsInAreasCount != MyManaFountainsInAreas.Count)
            {
                MyManaFountainsInAreas = new Dictionary<Circle, List<ManaFountain>>();

                myManaFountainsInArea = CalculateMyManaFountainsInArea(area);
                MyManaFountainsInAreas[area] = myManaFountainsInArea;
                MyManaFountainsInAreasCount = MyManaFountainsInAreas.Count;
            }
            else
            {
                if (!MyManaFountainsInAreas.TryGetValue(area, out myManaFountainsInArea))
                {
                    myManaFountainsInArea = CalculateMyManaFountainsInArea(area);
                    MyManaFountainsInAreas[area] = myManaFountainsInArea;
                    MyManaFountainsInAreasCount = MyManaFountainsInAreas.Count;
                }
            }

            return myManaFountainsInArea;
        }

        public List<ManaFountain> GetEnemyManaFountainsInArea(Circle area)
        {
            List<ManaFountain> enemyManaFountainsInArea = null;

            if (EnemyManaFountainsInAreas == null || EnemyManaFountainsInAreasCount != EnemyManaFountainsInAreas.Count)
            {
                EnemyManaFountainsInAreas = new Dictionary<Circle, List<ManaFountain>>();

                enemyManaFountainsInArea = CalculateEnemyManaFountainsInArea(area);
                EnemyManaFountainsInAreas[area] = enemyManaFountainsInArea;
                EnemyManaFountainsInAreasCount = EnemyManaFountainsInAreas.Count;
            }
            else
            {
                if (!EnemyManaFountainsInAreas.TryGetValue(area, out enemyManaFountainsInArea))
                {
                    enemyManaFountainsInArea = CalculateEnemyManaFountainsInArea(area);
                    EnemyManaFountainsInAreas[area] = enemyManaFountainsInArea;
                    EnemyManaFountainsInAreasCount = EnemyManaFountainsInAreas.Count;
                }
            }

            return enemyManaFountainsInArea;
        }
        #endregion
    }
}
