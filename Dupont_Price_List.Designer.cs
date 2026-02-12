using System.Data;

namespace Dupont_Price_Lists
{
    partial class Dupont_Price_List
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LabelCurrentItems = new Label();
            LabelOnlineItems = new Label();
            LabelNewPriceList = new Label();
            TextBoxCurrentItems = new TextBox();
            TextBoxNewPriceList = new TextBox();
            TextBoxOnlineItems = new TextBox();
            ButtonCurrentItems = new Button();
            ButtonOnlineItems = new Button();
            ButtonNewPriceList = new Button();
            Panel1 = new Panel();
            LabelPanel1Title = new Label();
            LabelPanel2Title = new Label();
            Panel2 = new Panel();
            LabelBrand = new Label();
            TextBoxMasterDiscountList = new TextBox();
            ComboBoxVendor = new ComboBox();
            LabelVendor = new Label();
            ComboBoxBrand = new ComboBox();
            LabelCategoryList = new Label();
            TextBoxCategoryList = new TextBox();
            ButtonMasterDiscountList = new Button();
            ButtonCategoryList = new Button();
            LabelMasterDiscountList = new Label();
            ButtonReadNewPriceList = new Button();
            ButtonReadMasterPriceList = new Button();
            LabelNewSKU = new Label();
            ComboBoxNewSKU = new ComboBox();
            ComboBoxNewDescription = new ComboBox();
            LabelNewDescription = new Label();
            ComboBoxNewListPrice = new ComboBox();
            LabelNewListPrice = new Label();
            ComboBoxNewUPC = new ComboBox();
            LabelNewUPC = new Label();
            ComboBoxNewWeight = new ComboBox();
            LabelNewWeight = new Label();
            ComboBoxNewBrand = new ComboBox();
            LabelNewBrand = new Label();
            CheckBoxUseField = new CheckBox();
            ButtonRetailUpdate = new Button();
            ButtonOnlineUpdate = new Button();
            LabelPanel3Title = new Label();
            DataGridViewMasterDiscountList = new DataGridView();
            PanelUpdateField = new Panel();
            DataGridViewRecord = new DataGridView();
            ProgressBarUpdate = new ProgressBar();
            ComboBoxNewDimention = new ComboBox();
            LabelNewDimention = new Label();
            ComboBoxNewFinish = new ComboBox();
            LabelNewFinish = new Label();
            TextStatus = new TextBox();
            Panel1.SuspendLayout();
            Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewMasterDiscountList).BeginInit();
            PanelUpdateField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewRecord).BeginInit();
            SuspendLayout();
            // 
            // LabelCurrentItems
            // 
            LabelCurrentItems.AutoSize = true;
            LabelCurrentItems.Location = new Point(55, 21);
            LabelCurrentItems.Name = "LabelCurrentItems";
            LabelCurrentItems.Size = new Size(82, 15);
            LabelCurrentItems.TabIndex = 0;
            LabelCurrentItems.Text = "Current items:";
            // 
            // LabelOnlineItems
            // 
            LabelOnlineItems.AutoSize = true;
            LabelOnlineItems.Location = new Point(60, 51);
            LabelOnlineItems.Name = "LabelOnlineItems";
            LabelOnlineItems.Size = new Size(77, 15);
            LabelOnlineItems.TabIndex = 1;
            LabelOnlineItems.Text = "Online items:";
            // 
            // LabelNewPriceList
            // 
            LabelNewPriceList.AutoSize = true;
            LabelNewPriceList.Location = new Point(53, 80);
            LabelNewPriceList.Name = "LabelNewPriceList";
            LabelNewPriceList.Size = new Size(84, 15);
            LabelNewPriceList.TabIndex = 2;
            LabelNewPriceList.Text = "New Price List:";
            // 
            // TextBoxCurrentItems
            // 
            TextBoxCurrentItems.Location = new Point(143, 18);
            TextBoxCurrentItems.Name = "TextBoxCurrentItems";
            TextBoxCurrentItems.Size = new Size(571, 23);
            TextBoxCurrentItems.TabIndex = 3;
            TextBoxCurrentItems.TextChanged += TextBoxCurrentItems_TextChanged;
            // 
            // TextBoxNewPriceList
            // 
            TextBoxNewPriceList.Location = new Point(143, 77);
            TextBoxNewPriceList.Name = "TextBoxNewPriceList";
            TextBoxNewPriceList.Size = new Size(571, 23);
            TextBoxNewPriceList.TabIndex = 4;
            TextBoxNewPriceList.TextChanged += TextBoxNewPriceList_TextChanged;
            // 
            // TextBoxOnlineItems
            // 
            TextBoxOnlineItems.Location = new Point(143, 48);
            TextBoxOnlineItems.Name = "TextBoxOnlineItems";
            TextBoxOnlineItems.Size = new Size(571, 23);
            TextBoxOnlineItems.TabIndex = 5;
            // 
            // ButtonCurrentItems
            // 
            ButtonCurrentItems.Location = new Point(730, 17);
            ButtonCurrentItems.Name = "ButtonCurrentItems";
            ButtonCurrentItems.Size = new Size(74, 23);
            ButtonCurrentItems.TabIndex = 6;
            ButtonCurrentItems.Text = "Select";
            ButtonCurrentItems.UseVisualStyleBackColor = true;
            ButtonCurrentItems.Click += ButtonCurrentItems_Click;
            // 
            // ButtonOnlineItems
            // 
            ButtonOnlineItems.Location = new Point(730, 47);
            ButtonOnlineItems.Name = "ButtonOnlineItems";
            ButtonOnlineItems.Size = new Size(74, 23);
            ButtonOnlineItems.TabIndex = 7;
            ButtonOnlineItems.Text = "Select";
            ButtonOnlineItems.UseVisualStyleBackColor = true;
            ButtonOnlineItems.Click += ButtonOnlineItems_Click;
            // 
            // ButtonNewPriceList
            // 
            ButtonNewPriceList.Location = new Point(730, 76);
            ButtonNewPriceList.Name = "ButtonNewPriceList";
            ButtonNewPriceList.Size = new Size(74, 23);
            ButtonNewPriceList.TabIndex = 8;
            ButtonNewPriceList.Text = "Select";
            ButtonNewPriceList.UseVisualStyleBackColor = true;
            ButtonNewPriceList.Click += ButtonNewPriceList_Click;
            // 
            // Panel1
            // 
            Panel1.BackColor = Color.FromArgb(255, 192, 192);
            Panel1.Controls.Add(LabelCurrentItems);
            Panel1.Controls.Add(TextBoxCurrentItems);
            Panel1.Controls.Add(TextBoxOnlineItems);
            Panel1.Controls.Add(TextBoxNewPriceList);
            Panel1.Controls.Add(LabelNewPriceList);
            Panel1.Controls.Add(ButtonOnlineItems);
            Panel1.Controls.Add(ButtonCurrentItems);
            Panel1.Controls.Add(ButtonNewPriceList);
            Panel1.Controls.Add(LabelOnlineItems);
            Panel1.Location = new Point(12, 37);
            Panel1.Name = "Panel1";
            Panel1.Size = new Size(834, 112);
            Panel1.TabIndex = 15;
            // 
            // LabelPanel1Title
            // 
            LabelPanel1Title.AutoSize = true;
            LabelPanel1Title.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabelPanel1Title.ForeColor = SystemColors.MenuHighlight;
            LabelPanel1Title.Location = new Point(23, 20);
            LabelPanel1Title.Name = "LabelPanel1Title";
            LabelPanel1Title.Size = new Size(99, 25);
            LabelPanel1Title.TabIndex = 16;
            LabelPanel1Title.Text = "Price Lists";
            // 
            // LabelPanel2Title
            // 
            LabelPanel2Title.AutoSize = true;
            LabelPanel2Title.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabelPanel2Title.ForeColor = SystemColors.MenuHighlight;
            LabelPanel2Title.Location = new Point(24, 165);
            LabelPanel2Title.Name = "LabelPanel2Title";
            LabelPanel2Title.Size = new Size(143, 25);
            LabelPanel2Title.TabIndex = 18;
            LabelPanel2Title.Text = "Reference Files";
            // 
            // Panel2
            // 
            Panel2.BackColor = Color.FromArgb(255, 192, 192);
            Panel2.Controls.Add(LabelBrand);
            Panel2.Controls.Add(TextBoxMasterDiscountList);
            Panel2.Controls.Add(ComboBoxVendor);
            Panel2.Controls.Add(LabelVendor);
            Panel2.Controls.Add(ComboBoxBrand);
            Panel2.Controls.Add(LabelCategoryList);
            Panel2.Controls.Add(TextBoxCategoryList);
            Panel2.Controls.Add(ButtonMasterDiscountList);
            Panel2.Controls.Add(ButtonCategoryList);
            Panel2.Controls.Add(LabelMasterDiscountList);
            Panel2.Location = new Point(13, 182);
            Panel2.Name = "Panel2";
            Panel2.Size = new Size(833, 142);
            Panel2.TabIndex = 17;
            // 
            // LabelBrand
            // 
            LabelBrand.AutoSize = true;
            LabelBrand.Location = new Point(92, 80);
            LabelBrand.Name = "LabelBrand";
            LabelBrand.Size = new Size(41, 15);
            LabelBrand.TabIndex = 12;
            LabelBrand.Text = "Brand:";
            // 
            // TextBoxMasterDiscountList
            // 
            TextBoxMasterDiscountList.Location = new Point(142, 47);
            TextBoxMasterDiscountList.Name = "TextBoxMasterDiscountList";
            TextBoxMasterDiscountList.Size = new Size(571, 23);
            TextBoxMasterDiscountList.TabIndex = 11;
            TextBoxMasterDiscountList.TextChanged += TextBoxMasterDiscountList_TextChanged;
            // 
            // ComboBoxVendor
            // 
            ComboBoxVendor.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxVendor.FormattingEnabled = true;
            ComboBoxVendor.Items.AddRange(new object[] { "", "&Tradition", "(CBH)-Canadian Builders Hardware ", "2nd Ave Lighting", "A & B Floral", "A&B Home Inc.", "Abbott", "Abigails", "ABT Fishing", "Abyss and Habidecor", "ACO", "AD Waters", "Aero Mist", "AES Hearth & Patio", "AHM Design", "Aico Furniture", "akk91-05", "Aktuell", "Alessi", "Alfa Goma", "alibaba", "Alico", "Alkota", "All Clad", "Allegri", "Allsop Home Garden", "Almo", "Alora", "Altech Electronics", "Amana", "Amantii", "Amba", "Ambiance Fireplaces and Grills", "Amercraft Inc.", "American Lighting", "American Standard", "Americh", "AmeriVent", "AMES", "Analytical Industries", "Annieglass", "Antique Collectors Club", "APR Supply Co", "Aptus", "Aqua", "Aquadesign", "Aquiform Distributors", "Archgard", "Archipelago Botanicals", "ARCHITECTMADE", "Arctic Spas", "Areo Mist", "Armar Tile", "Armco Agencies Inc.", "Artcraft Lighting", "Arte Italica", "Artel", "Arteriors", "ASH", "S FEBRUARY PO's HERE ", "Ashleigh Manor", "Ashley Furniture", "ASH'S APRIL PO'S", "ASH'S JANUARY 2020 PO'S", "ASH's MARCH 2020", "ASH's MAY PO's", "ASH'S PO#'S GO HERE !!!!!!!", "asi", "ASI- Canada ", "Ateco", "Athens Stonecasting", "AVAD", "Axent", "B K Resources - AB Canadian Distribution Inc", "BA Robinson", "Bad Boy Mower", "Bagno Italia", "Bailey Street", "Bain Signature", "Bainbridge Vet (AU)", "BAINULTRA", "Barclay Products", "bardo", "Bardon Supplies", "Barefoot Dreams", "Barens Inc.", "Baril", "Barron Lighting", "Barry E. Walter Sr. Co.", "Bathworks", "Beatriz Ball", "Beekman BV", "Beistle Co.", "Belanger", "Bells of Vienna", "Belwith Keeler", "Bemis", "Berenson", "Besa Lighting", "Better Life", "BIA Cordon Bleu", "Big Ass Fans", "Big Dipper Wax Works", "BIOS Professional", "BlackJack Lighting", "Blanco", "Blaze King", "Blevins Inc", "Bloomingville", "Blue Ridge Knives", "BN Products-USA, LLC.", "Bob-Cat", "Bocchi", "Bodum", "Bormioli Rocco", "Bosch", "Bosco Canada", "Botanico", "Bovi", "Bradford White Water Heaters", "Bramble", "BRASSTECH, NEWPORT, GINGER", "Breezesta", "Brennan", "Breville", "Brighton", "Brizo", "BROAN", "Brondell", "Browne & Co.", "Brownstone Distribution", "BUILDERS TOOLS SUPPLIES LTD.", "Bulbrite", "Cabano", "Cabinetsmith", "Calaisio", "CALIFORNIA FAUCET", "Calphalon", "Cal-Steam", "Candym", "Canfloyd", "Cangshan", "Canvas", "Capdeco", "Capital Lighting", "Capri Blue", "Caroma", "Carson Home Industries", "Casabella", "Casablanca Fan Co", "Casamance", "cb", "CB Supplies", "CDN", "Centennial Plumbing Supply ", "Centura Tile", "CFC", "ch", "cha", "Chantal", "char", "chard", "Chard Marketing", "Charles McMurray", "Charles Viancin", "Chef'n", "Chef's Choice", "Cheviot", "Cheviot Products", "Chewing the Cud", "Chicago Faucets", "ChicWrap", "china", "Clarke and Clarke", "Classic Brand Cabinetry", "Classic Home", "Clawfoot Design", "CLM Distribution", "Clock Audio North America", "CMT Industrial Solutions", "Coast Cutlery Co", "Coaster", "Coldture", "Cole and Mason", "Colorado Dallas", "Company C", "Conbraco ", "Concord", "Control4", "Cooee Design", "Cookie Cutter", "CORAM Tools USA", "Corbett Lighting", "Corkcicle", "Coton Colors", "Counter Art", "Coyuchi", "CR Gibson", "Craftmade", "Craftsmens", "Creative Co-op", "Creative System Lighting (CSL)", "Cristel", "Crown Mark", "Crystorama", "CT1", "CTW Home Collection", "Cuisinox", "Cuisipro", "Cupcakes and Cartwheels", "CURIO", "Currey & Company", "Cusinart", "Cyan Design", "Dahl", "Dale Tiffany", "Dalyn Rug Company", "Danamark Watercare (Pentair)", "Danby", "Danesco", "Danica Studios", "Danson", "Dash and Albert", "David Shaw Designs", "Dax International", "DayMen", "Deborah Rhodes", "Deco Breeze", "Decorative Plumbing Distributors", "Decorsense", "Delete", "Delta/ Masco", "Desco", "Deshouliers", "Design Center", "Design Design", "Design Ideas", "Designers Fountain", "Designers Guild (Canada)", "Designers Guild UK", "Desperate Signs", "Diamond Lighting", "Diamond Sofa", "Dimplex", "DiodeLED", "Direct Distributors / Mustee", "Divinity Boutique", "DM Bath", "Dobbin Sales", "DOIY", "Dolan Designs", "Dolmar", "Doors and More", "Dornbracht", "Dot Line Corp.", "Down to Earth Distributors Inc.", "DPM Fragrance", "Dragon Fire", "Dress My Cupcake", "Drill America", "Dulux (AU)", "Dupont", "Dupont Plumbing ", "Duralex", "Duravit", "DVI Lighting", "DW Windows", "Dyconn", "Dyna-King", "DynoCams", "Earphone Connection", "East of India", "Eastern Accents", "Eastman EZ-FLO", "EASYPHIX", "Eaton Aeroquip", "E-Cloth", "Ecotimber", "ECOWAY", "Edenborough", "EGLO Lighting", "Elan Lighting", "Electric Mirror", "Elegant Lighting", "Element4", "Elk Lighting", "ELKAY SUMMER", "Emco", "Emerson", "Epicurean", "Escali", "EStainless", "Et Al Designs", "ET2", "eTech Parts", "Ethnicraft", "Eurofase", "Europe2you", "Fabula", "Fairmont", "Fairmont Design's", "Fanimation", "Fanlight Co.", "Fedex", "Ferm Living", "Fine Decor UK", "Firetainment", "Fishs Eddy", "Fiskars", "Flare", "Flash Technologies LLC", "Fleurco", "Florida Hardware Company", "Florida Pneumatic", "Foremost Canada", "Fox Run Brands", "Fox Run Brands (Canada)", "Franke / Kindred", "Fredrick Ramond Lighting", "Freelance Delivery Drivers", "Frieling", "Frigidaire", "Full Circle", "Futura Industries", "G.F. Thompson", "Galaxy Lighting", "Ganesh Himal", "Gatco", "Geberit", "Geier Glove Co", "Gena Decor Inc", "General Plumbing", "Generation Lighting", "George Kovacs", "Gerber Gear", "Gien France", "Gladiator", "Gloves For Professionals", "Golden Lighting", "Golden Spire Vanties", "Gourmet du Village", "GRAFF Designs", "Graham & Brown", "Grant Products", "Graphique de France", "Gravely", "Green Goo", "Grohe", "Guzzini", "Halco", "Hammerton", "Handley House", "HansGrohe", "Hardware Resources", "Harman Stoves", "Harold Import Co.", "Hawkins New York", "HCP Ceramics Co.", "HD Supply", "HealthCraft", "Hearth & Home Technologies", "Hearthstone", "Heat & Glo", "Heath", "Helinox", "Hen House Linens", "Hero's Pride", "Hertz", "Hester & Cook", "Heymat", "HG international BV", "HG Systems", "Hida Tools", "Hidden", "HiEnd Accents", "Hillhouse Naturals", "Hillman", "Hinkley Lighting", "Hitachi Koki U.S.A. Ltd.", "HK Living", "Holten Impex ", "HomArt", "Home Refinements", "Homelegance", "Hometown Bath & Body", "Homey Design Furniture", "Honda", "Hooker Furniture", "Hotsy", "House Doctor", "House of Troy", "Houser Racing", "Howard Berger Co.", "Howard Elliott", "Howard Miller", "Hubbardton Forge", "Hudson Valley Lighting", "Hunter Ceiling Fan", "Hustler Turf", "Hydro Systems", "Hydro-Blok", "Hydro-Rain", "Icera", "ICO", "ICP Adhesives & Sealants", "Illume", "IMAX", "IMG", "Imprint Mats", "Improv Electronics", "In Common With", "In2aqua", "Indaba", "India Handicraft Exporter", "Infinity", "Infinity Drain", "Ingram Micro", "INNERSPACE Luxury Products", "InSinkErator", "Integrated Supply Network", "InterDesign", "Interline Brands", "International Lighting", "Intimidator 4x4 Utility Vehicles", "Invisia", "iRobot", "Isenberg", "It's About RoMI", "Jade Time", "Jambanz", "James Crapper Plumbing Sales", "James R. Moder", "Jameson", "Jamie Oliver", "JBL", "JD Lighting", "Jenn-Air", "Jensen Distribution", "Jensen Leisure", "JMS EuroCanada", "John Robshaw Textiles", "Johnson Hardwood", "Johnson Level", "Jotul", "Juliska", "jura CAPRESSO", "Justice Design", "K", "K. Hall Designs", "KAF", "Kalalou", "Kalco Lighting", "Kalia", "Kallista", "Kalon Studio", "Kanrep Norris Distribution", "Kartners", "Kassatex", "Kay Dee Design", "Kazi", "Ken Hom", "Kendal", "Kenroy Home", "Keson", "Kichler Landscape", "Kichler Lighting", "King Hickory", "Kingsman Fireplaces", "Kinnex", "Kiss That Frog", "KitchenAid", "klei", "KLEIN", "Kliman Sales", "Kohler", "Kohler Co.", "Kollezi", "Kontrol UK", "Korky", "Kozy Heat Fireplaces", "Kraft Tool Co.", "Kristina Dam Studio", "KubeBath", "Kuhn Rikon", "Kurt S. Adler", "Kuzco Lighting (Canada)", "Kuzco Lighting Inc.", "KWC", "Kyocera Advanced Ceramics", "La Siesta", "Labell Inc. Canada", "LACAVA", "Lafco", "LaLOO Accessories Inc.", "Lampe Berger", "Landmark Lighting", "LASCO", "Laser Reference Inc.", "Lassen", "Laufen", "LBL Lighting", "Le cadeaux", "LE CREUSET", "Leader Products", "Leather Italia USA", "Leatherman", "Legacy Hardwood & Trims", "Legacy Manufacturing Co.", "Legrand Lighting", "Leica Camera AG", "Lenova", "Lenox", "Les Artistes", "Lifestyle Home Collection (EUR)", "LightingOne", "Linie Design", "Lite Source", "Little Wonder", "Livex Lighting", "LMT Rustic", "Logrite", "Loll Designs", "Longust", "Louis Poulsen", "Lowepro", "Lucuma Designs", "Lukx", "Lyncar", "M2 Tile & Stone", "Maax", "Mackie", "Magenta Inc.", "Majestic Marble Import", "Majestic Products", "Makita (AU)", "Makita USA Inc.", "Mann Marketing", "Mansfield", "Margot Elena", "Marine Wholesale", "Marquis Fireplaces", "Marshalltown", "Marvin", "Master Plumber", "Mater", "Matteo Lighting (Canada)", "Matthews Fan Company", "Matthews Fan Company II", "Maxim Lighting", "Maxxima", "Maytag", "M-D Building Products Inc.", "Mech Tech", "Mediterranea", "Melrose International", "Menu", "Mercana", "Mercer Culinary", "Metropolitan", "Meyda Tiffany", "Meyer Plumbing Supply", "MGS Milano", "Michel Design Works", "MidWest Fastener", "Milton Industries", "Milwaukee Electric Tool Corp.", "Minka Aire", "Minka Lavery", "Mirolin", "Mitzi Lighting", "MK Morse", "Mobilesentrix", "Moda at Home", "Modern Decor Building Supplies", "Modern Fan Company", "Modern Forms Fans", "Modern Forms Lighting", "Modern-twist", "Modus", "Moen", "Monte Carlo", "Mountain Plumbing Products", "Mr. Marble", "Mr. Steam", "Mrs. Meyer's Clean Day", "MTI", "MURALUXE", "Murray Feiss", "Muuto", "My Phoenix Accessory", "Nambe", "Nanolux", "Nantucket Sinks", "NAPA Home & Garden", "Native Trails", "NEBO Tools", "Neptune", "Neptune Rouge", "New Edition (EUR)", "Newborn", "Newgen Sales", "Next Supply", "Nichols Bros Stoneworks", "Noble Trade - Headoffice ", "Noble Trade Montreal", "Nora Fleming", "Nora Fleming", "Nordic Ware", "Norintel", "Normann Copenhagen", "Normode", "Nortesco / Streamline Canada", "NorthCape", "Northwest Stoves Ltd", "Northwood Mirrors", "Norwell", "Novanni Stainless Inc. ", "Now Designs", "NuVue", "OAKS", "Oatey", "Oatey Quick Drain", "Oceania", "Olde Thompson", "Olliix", "one", "Ontario Knife Company", "ORE Originals", "Orgill, Inc.", "OS&B", "Ove", "OXO", "Oxygen", "p", "Pablo Designs", "Pace Supply", "Pacific Cost Lighting", "Pacific Merchants", "PACOA", "Paderno", "Page One", "Panasonic", "Paper Products Design", "Park Designs", "Peacock Alley", "Pearl Abrasive", "Peking Handicraft", "Perfect Choice", "Perlick", "Perrin & Rowe", "Pfister", "Philmac (AU)", "Phylrich", "PierDeco", "Pine Cone Hill", "Pink & Brown", "Planet Wise", "Plumb Line Sales Limited", "Plumbery Distribution", "Plumbing Centre", "PMF", "PMG", "Polar Electronics", "Polywood", "Pom Pom At Home", "Pomeroy", "Poo Pourri", "Porcemall Tile", "Port 68", "Port Style Enterprises", "Pressure-Pro", "Prestigious Textiles UK", "Primelite", "Prochef", "Produits Neptune", "Prodyne", "Progress Lighting", "Progressive", "Puka Creation", "Qhousekids", "Quadra-Fire", "Quoizel", "Quorum", "R & D Energy Savers LTD", "R.H. Peterson Co.", "R23", "r394drch", "RareEssence", "Raymarine", "Red and White Kitchen", "Regency", "Regency Fireplace Products", "Regina Andrew Design", "Reiss Wholesale Hardware", "Renaissance Rumford", "RenWil", "Restonic Mattresses", "Rhythm Clock Company", "Riedel", "Riobel Inc", "Robbie & Berking", "Robern", "Robert Abbey", "Robot Coupe", "Rock Solid", "Rohl", "Rokform", "Ronbow", "Room It Up", "RoomMates", "Rosanna", "Rosendahl Design Group", "Rosle", "Rosy Rings", "Rotary Corporation", "Royal Bath & Marble", "Royal Limoges", "RSF Fireplaces", "RSVP International", "rub", "RUBI", "rubin", "Rubinet", "Runtal", "RyFab Industries", "Sabre Flatware", "Safety Maker Inc", "Sage Kitchen & Bath", "Sandcliff Sales", "Sanderson", "Saniflo", "Satco", "Savoy House Lighting", "Schluter", "Scooterworks USA", "SeaGull Lighting", "Seaside Casual", "SecondLife Inkjets", "Senco", "Serax", "Serrv", "Sharp", "Sheffield Group (AU)", "Shiraleah", "SHUN", "Sid the Kid's Express Delivery", "Sidler", "Sidler", "Sign of the Crab", "Silky Saws", "Sioux Chief", "Sir/Madam", "Skagerak USA", "Skandinavisk", "Skullcandy", "Skyros", "Skywalker", "Slik/Tomlin ", "Smith South Central", "SnapAV", "Soda Stream", "SolisTek", "Sonneman", "Sonoma Forge", "Southwest Moulding Company", "Soxs", "SP Connect ", "St. Francis Herbs Farm", "Stanley Black & Decker", "Stanley National", "Steamist", "SteamSauna", "Steiner", "Stens", "Steve Smith sales", "Steven Willand Inc.", "Stihl", "Stoll Fireplace", "Stone Forest", "stonewood", "Strasser", "Strom Plumbing ", "Style Library", "Stylish", "SUA International", "Suite Bebe", "Sunny Designs", "Sunpan", "Sunrise Specialty", "Superchem Industries", "Supreme Fireplaces", "Supreme Housewares", "Swan Creek", "Swit", "Swizz Style Inc.", "Synchronicity", "TableTopics", "Talking Tables", "Taps & Tubs", "Tech Candy", "Tech Lighting", "Tecnobrass", "TEMCO", "Tenergy", "The Emery-Waterhouse Company", "The Piggy Story", "The Plumbery", "The Plumbing Connection", "ThermaSol", "THR BV", "Tidal", "Tolindo Deliveries ", "Toltec Lighting", "TOPCAN", "Torre & Tagus", "Totally Bamboo", "TOTO", "Travis Industries", "Triple S Sporting Supplies", "Trison Sales", "Tropitone", "Troy Lighting", "TruCut Inc", "Tubco", "Two's Company", "Typhoon", "Ukinox", "Uline", "UMA Enterprises", "Unior USA", "Universal Wholesale", "US Floors", "USA Pans", "Uttermost", "V&V Appliance Parts", "Vagabond House", "Valor Fireplaces", "Varaluz", "Vast Inc", "Vaxcel", "Veneto/ Muti Vanities ", "Vermont Castings", "Vertical Supply Group", "VGT", "Victoria + Albert", "Victorinox Knives", "Viega", "Vietri", "Vifa", "Viking Distributing East", "Villa", "Villeroy & Boch", "Vinidex", "Visual Comfort", "VOGT INDUSTRIES", "Vollrath", "Voluspa", "Votivo", "VPI Corp", "Wac Lighting", "WallWalker", "WALLY'S PO ", "WarmUp", "Waste King", "Watco ", "Watermark Designs", "Watts", "Weaver Leather", "Wee Gallery", "Weil-Mclain", "West Gate", "Western Sling", "Westgate", "Westinghouse Lighting", "WHCI Plumbing Supply", "Whirlpool", "Whiskey Mountain", "Whitewood", "Wholesale Gadget Parts", "Wild & Wolf", "Wild Wings", "Wildly Delicious", "Willis / Artisan", "Wilton", "WInd River", "Winners Only", "Winward", "Wittus", "Wolf Peak International Inc", "World Buyers", "WOUD", "Wright Tool", "WSB Cabinet Hardware", "Wusthof", "Yamaha", "Yamaha (NL)", "York Wallcoverings", "Zacuto", "Zitta", "Z-Lite", "Zodax", "ZVELT & DESIGN Inc.", "ZWILLING", "ZWILLING J.A. Henckels Canada", "Zyliss" });
            ComboBoxVendor.Location = new Point(142, 107);
            ComboBoxVendor.Name = "ComboBoxVendor";
            ComboBoxVendor.Size = new Size(266, 23);
            ComboBoxVendor.TabIndex = 10;
            // 
            // LabelVendor
            // 
            LabelVendor.AutoSize = true;
            LabelVendor.Location = new Point(86, 110);
            LabelVendor.Name = "LabelVendor";
            LabelVendor.Size = new Size(47, 15);
            LabelVendor.TabIndex = 9;
            LabelVendor.Text = "Vendor:";
            // 
            // ComboBoxBrand
            // 
            ComboBoxBrand.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxBrand.FormattingEnabled = true;
            ComboBoxBrand.Items.AddRange(new object[] { "", "AB & A", "ACO", "ACRITEC", "AHM", "Akcess 2.0", "Akt", "Aktuell", "Alcove", "ALT", "Amba", "Amercraft inc.", "American Standard", "Aqua", "Aquabrass", "Aquadesign", "Aquadis", "Aqualem", "Aquatown", "Aquor", "Aria", "Armco", "Armstrong Pumps", "Artisan", "asi- canada", "Avenue", "B K Resources", "Bacia", "Bagno Italia", "Bain Signature", "BainUltra", "Barclay", "Bardon", "Baril", "Basico", "Belanger", "Bemis", "Blanco", "Bobrick", "Bosco", "Bradford White", "Bradley", "Brand", "BrassCraft", "Brizo", "Brondell", "Bronte", "Cabano", "Cabinetsmith", "Cambridge Brass", "caml", "Caroma", "Catalano", "Cavalli", "cb", "CB Supplies", "CBH - Canadian Builders Hardware", "Centaco", "Ceska", "Cheviot", "chicago", "Chicago Faucets", "CHURCH", "Classic Brand Cabinetry", "ClawFoot Design", "Conbr", "Conbraco", "Contesor", "Contrac", "Crystal Mountain", "Dahl", "decolav", "Delta", "desco", "Disegno", "DM Bath", "Doors and More", "Dupont", "Dupont Plumbing", "Duravit", "Eastman EZ-FLO", "Eclipse", "ECOWAY", "Emmevi", "Essentials", "Eurofase", "Fairmont Design&#039;s", "Fiat", "Fibo", "Fiora", "Fleurco", "FluidMaster", "Folo", "Franke", "GASTITE", "Geberit", "Gena", "General", "General Pipe Cleaners", "Gerber", "Golden Spire", "Grohe", "GSI", "hans", "Hansa", "Hansgrohe", "Harold Import Company Inc.", "HealthCraft", "HG Systems", "Holten Impex International", "Horus", "House of Rohl", "HUDSON VALLEY LIGHTING", "Hydro-Blok", "IBB", "Icera", "ICO", "Ikonik", "Ikonik Bypass", "img", "Incepa", "Inda", "InSinkErator", "Invisia", "JMS", "John Wood", "Kalia", "Kanrep", "Kareo", "Kerasan", "Kindred", "Kingston Brass", "klein", "Kohler", "Kollezi", "Koncept Evo Film", "Korky", "Kosla", "KubeBath", "Kugler", "KWC", "la torre", "LaLOO", "latorre", "Laufen", "lenova", "Liberty Pumps", "LUKX", "Luna", "Lyncar", "MAAX", "Maier", "Main line", "Mansfield", "Master Plumber", "Masters", "MAYFAIR", "Mirolin", "moen", "Moroka", "Mountain Plumbing", "Mr Marble", "Mr. Marble", "Mr. Steam", "MTC", "MURALUXE", "Mustee", "MUTI", "Nantucket", "Native Trails", "Neptune", "New Gen Sales", "Newform", "Newport Brass", "noble Trade", "Nortesco", "Northwood", "Novanni", "o pro", "Oaks", "Oatey", "OATEY/HERCULES", "Oceania", "OLI", "Olsonite", "OS&amp;B", "Ottinetti", "Panasonic", "Pasco", "Pentair", "Perrin &amp; Rowe", "Pfister", "PierDeco", "Plumbco", "powers", "ProChef", "Rainb&#39;o", "REGINOX", "Rheem", "Rigid", "Rinnai", "rio", "Riobel", "Robern", "Robertshaw", "ROHL", "Ronbow", "Roundone", "Royal Bath &amp; Marble", "RUBI", "Rubinet", "Saniflo", "Saunacore", "Schluter", "Shaws", "Sidler", "Sign Of the Crab", "Simas", "Sioux Chief", "Slik", "Sloan", "Sluyter", "Sonia", "Sonos", "Squareone", "St Thomas Creations", "SteamSauna", "Stelrad", "Sterling", "Stonetouch", "Stonewood", "Storm Drain Plus", "Strom Plumbing", "Stylish", "Superchem Industries", "Symmons", "Tebisa Faucets", "TecnoBrass", "Tenzo", "Thermasol", "Tidal", "Tolindo", "Top", "TopCan", "TOTO", "Trap Guard", "Tubco", "Uline", "Urba", "Veneto", "Victoria + Albert", "Villeroy &#38; Boch", "VOGT", "voht", "WALTEC", "WarmUp", "Watco", "Waterstone", "Watts", "Wilde", "Woodford", "ZITTA", "Zoeller", "Zomodo", "Zucchetti", "Zurn", "Zvelt" });
            ComboBoxBrand.Location = new Point(142, 77);
            ComboBoxBrand.Name = "ComboBoxBrand";
            ComboBoxBrand.Size = new Size(266, 23);
            ComboBoxBrand.TabIndex = 8;
            // 
            // LabelCategoryList
            // 
            LabelCategoryList.AutoSize = true;
            LabelCategoryList.Location = new Point(54, 20);
            LabelCategoryList.Name = "LabelCategoryList";
            LabelCategoryList.Size = new Size(79, 15);
            LabelCategoryList.TabIndex = 0;
            LabelCategoryList.Text = "Category List:";
            // 
            // TextBoxCategoryList
            // 
            TextBoxCategoryList.Location = new Point(142, 17);
            TextBoxCategoryList.Name = "TextBoxCategoryList";
            TextBoxCategoryList.Size = new Size(571, 23);
            TextBoxCategoryList.TabIndex = 3;
            // 
            // ButtonMasterDiscountList
            // 
            ButtonMasterDiscountList.Location = new Point(729, 46);
            ButtonMasterDiscountList.Name = "ButtonMasterDiscountList";
            ButtonMasterDiscountList.Size = new Size(74, 23);
            ButtonMasterDiscountList.TabIndex = 7;
            ButtonMasterDiscountList.Text = "Select";
            ButtonMasterDiscountList.UseVisualStyleBackColor = true;
            ButtonMasterDiscountList.Click += ButtonMasterDiscountList_Click;
            // 
            // ButtonCategoryList
            // 
            ButtonCategoryList.Location = new Point(729, 16);
            ButtonCategoryList.Name = "ButtonCategoryList";
            ButtonCategoryList.Size = new Size(74, 23);
            ButtonCategoryList.TabIndex = 6;
            ButtonCategoryList.Text = "Select";
            ButtonCategoryList.UseVisualStyleBackColor = true;
            ButtonCategoryList.Click += ButtonCategoryList_Click;
            // 
            // LabelMasterDiscountList
            // 
            LabelMasterDiscountList.AutoSize = true;
            LabelMasterDiscountList.Location = new Point(16, 49);
            LabelMasterDiscountList.Name = "LabelMasterDiscountList";
            LabelMasterDiscountList.Size = new Size(117, 15);
            LabelMasterDiscountList.TabIndex = 1;
            LabelMasterDiscountList.Text = "Master Discount List:";
            // 
            // ButtonReadNewPriceList
            // 
            ButtonReadNewPriceList.Location = new Point(13, 332);
            ButtonReadNewPriceList.Name = "ButtonReadNewPriceList";
            ButtonReadNewPriceList.Size = new Size(136, 23);
            ButtonReadNewPriceList.TabIndex = 19;
            ButtonReadNewPriceList.Text = "Read New Price List";
            ButtonReadNewPriceList.UseVisualStyleBackColor = true;
            ButtonReadNewPriceList.Click += ButtonReadNewPriceList_Click;
            // 
            // ButtonReadMasterPriceList
            // 
            ButtonReadMasterPriceList.Location = new Point(155, 332);
            ButtonReadMasterPriceList.Name = "ButtonReadMasterPriceList";
            ButtonReadMasterPriceList.Size = new Size(179, 23);
            ButtonReadMasterPriceList.TabIndex = 20;
            ButtonReadMasterPriceList.Text = "Read Master Discount List";
            ButtonReadMasterPriceList.UseVisualStyleBackColor = true;
            ButtonReadMasterPriceList.Click += ButtonReadMasterPriceList_Click;
            // 
            // LabelNewSKU
            // 
            LabelNewSKU.AutoSize = true;
            LabelNewSKU.Location = new Point(64, 371);
            LabelNewSKU.Name = "LabelNewSKU";
            LabelNewSKU.Size = new Size(31, 15);
            LabelNewSKU.TabIndex = 13;
            LabelNewSKU.Text = "SKU:";
            // 
            // ComboBoxNewSKU
            // 
            ComboBoxNewSKU.FormattingEnabled = true;
            ComboBoxNewSKU.Location = new Point(99, 368);
            ComboBoxNewSKU.Name = "ComboBoxNewSKU";
            ComboBoxNewSKU.Size = new Size(322, 23);
            ComboBoxNewSKU.TabIndex = 13;
            // 
            // ComboBoxNewDescription
            // 
            ComboBoxNewDescription.FormattingEnabled = true;
            ComboBoxNewDescription.Location = new Point(99, 397);
            ComboBoxNewDescription.Name = "ComboBoxNewDescription";
            ComboBoxNewDescription.Size = new Size(322, 23);
            ComboBoxNewDescription.TabIndex = 21;
            // 
            // LabelNewDescription
            // 
            LabelNewDescription.AutoSize = true;
            LabelNewDescription.Location = new Point(23, 400);
            LabelNewDescription.Name = "LabelNewDescription";
            LabelNewDescription.Size = new Size(70, 15);
            LabelNewDescription.TabIndex = 22;
            LabelNewDescription.Text = "Description:";
            // 
            // ComboBoxNewListPrice
            // 
            ComboBoxNewListPrice.FormattingEnabled = true;
            ComboBoxNewListPrice.Location = new Point(99, 426);
            ComboBoxNewListPrice.Name = "ComboBoxNewListPrice";
            ComboBoxNewListPrice.Size = new Size(322, 23);
            ComboBoxNewListPrice.TabIndex = 23;
            // 
            // LabelNewListPrice
            // 
            LabelNewListPrice.AutoSize = true;
            LabelNewListPrice.Location = new Point(36, 429);
            LabelNewListPrice.Name = "LabelNewListPrice";
            LabelNewListPrice.Size = new Size(57, 15);
            LabelNewListPrice.TabIndex = 24;
            LabelNewListPrice.Text = "List Price:";
            // 
            // ComboBoxNewUPC
            // 
            ComboBoxNewUPC.FormattingEnabled = true;
            ComboBoxNewUPC.Location = new Point(99, 455);
            ComboBoxNewUPC.Name = "ComboBoxNewUPC";
            ComboBoxNewUPC.Size = new Size(322, 23);
            ComboBoxNewUPC.TabIndex = 25;
            // 
            // LabelNewUPC
            // 
            LabelNewUPC.AutoSize = true;
            LabelNewUPC.Location = new Point(60, 458);
            LabelNewUPC.Name = "LabelNewUPC";
            LabelNewUPC.Size = new Size(33, 15);
            LabelNewUPC.TabIndex = 26;
            LabelNewUPC.Text = "UPC:";
            // 
            // ComboBoxNewWeight
            // 
            ComboBoxNewWeight.FormattingEnabled = true;
            ComboBoxNewWeight.Location = new Point(99, 484);
            ComboBoxNewWeight.Name = "ComboBoxNewWeight";
            ComboBoxNewWeight.Size = new Size(322, 23);
            ComboBoxNewWeight.TabIndex = 27;
            // 
            // LabelNewWeight
            // 
            LabelNewWeight.AutoSize = true;
            LabelNewWeight.Location = new Point(47, 487);
            LabelNewWeight.Name = "LabelNewWeight";
            LabelNewWeight.Size = new Size(48, 15);
            LabelNewWeight.TabIndex = 28;
            LabelNewWeight.Text = "Weight:";
            // 
            // ComboBoxNewBrand
            // 
            ComboBoxNewBrand.FormattingEnabled = true;
            ComboBoxNewBrand.Location = new Point(99, 577);
            ComboBoxNewBrand.Name = "ComboBoxNewBrand";
            ComboBoxNewBrand.Size = new Size(235, 23);
            ComboBoxNewBrand.TabIndex = 29;
            // 
            // LabelNewBrand
            // 
            LabelNewBrand.AutoSize = true;
            LabelNewBrand.Location = new Point(52, 580);
            LabelNewBrand.Name = "LabelNewBrand";
            LabelNewBrand.Size = new Size(41, 15);
            LabelNewBrand.TabIndex = 30;
            LabelNewBrand.Text = "Brand:";
            // 
            // CheckBoxUseField
            // 
            CheckBoxUseField.AutoSize = true;
            CheckBoxUseField.Location = new Point(348, 579);
            CheckBoxUseField.Name = "CheckBoxUseField";
            CheckBoxUseField.Size = new Size(73, 19);
            CheckBoxUseField.TabIndex = 31;
            CheckBoxUseField.Text = "Use Field";
            CheckBoxUseField.UseVisualStyleBackColor = true;
            CheckBoxUseField.CheckedChanged += CheckBoxUseField_CheckedChanged;
            // 
            // ButtonRetailUpdate
            // 
            ButtonRetailUpdate.BackColor = Color.IndianRed;
            ButtonRetailUpdate.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ButtonRetailUpdate.ForeColor = SystemColors.Control;
            ButtonRetailUpdate.Location = new Point(9, 11);
            ButtonRetailUpdate.Name = "ButtonRetailUpdate";
            ButtonRetailUpdate.Size = new Size(182, 41);
            ButtonRetailUpdate.TabIndex = 32;
            ButtonRetailUpdate.Text = "Retail Update File";
            ButtonRetailUpdate.UseVisualStyleBackColor = false;
            ButtonRetailUpdate.Click += ButtonRetailUpdate_Click;
            // 
            // ButtonOnlineUpdate
            // 
            ButtonOnlineUpdate.BackColor = Color.IndianRed;
            ButtonOnlineUpdate.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ButtonOnlineUpdate.ForeColor = SystemColors.Control;
            ButtonOnlineUpdate.Location = new Point(211, 11);
            ButtonOnlineUpdate.Name = "ButtonOnlineUpdate";
            ButtonOnlineUpdate.Size = new Size(185, 41);
            ButtonOnlineUpdate.TabIndex = 33;
            ButtonOnlineUpdate.Text = "Online Update File";
            ButtonOnlineUpdate.UseVisualStyleBackColor = false;
            ButtonOnlineUpdate.Click += ButtonOnlineUpdate_Click;
            // 
            // LabelPanel3Title
            // 
            LabelPanel3Title.AutoSize = true;
            LabelPanel3Title.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabelPanel3Title.ForeColor = SystemColors.MenuHighlight;
            LabelPanel3Title.Location = new Point(23, 626);
            LabelPanel3Title.Name = "LabelPanel3Title";
            LabelPanel3Title.Size = new Size(192, 25);
            LabelPanel3Title.TabIndex = 36;
            LabelPanel3Title.Text = "Master Discount List";
            // 
            // DataGridViewMasterDiscountList
            // 
            DataGridViewMasterDiscountList.BackgroundColor = SystemColors.HighlightText;
            DataGridViewMasterDiscountList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewMasterDiscountList.Location = new Point(12, 670);
            DataGridViewMasterDiscountList.Name = "DataGridViewMasterDiscountList";
            DataGridViewMasterDiscountList.Size = new Size(833, 139);
            DataGridViewMasterDiscountList.TabIndex = 37;
            // 
            // PanelUpdateField
            // 
            PanelUpdateField.BackColor = Color.White;
            PanelUpdateField.BorderStyle = BorderStyle.FixedSingle;
            PanelUpdateField.Controls.Add(DataGridViewRecord);
            PanelUpdateField.Controls.Add(ButtonRetailUpdate);
            PanelUpdateField.Controls.Add(ButtonOnlineUpdate);
            PanelUpdateField.Location = new Point(440, 347);
            PanelUpdateField.Name = "PanelUpdateField";
            PanelUpdateField.Size = new Size(406, 304);
            PanelUpdateField.TabIndex = 38;
            // 
            // DataGridViewRecord
            // 
            DataGridViewRecord.BackgroundColor = SystemColors.InactiveBorder;
            DataGridViewRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewRecord.Location = new Point(9, 65);
            DataGridViewRecord.Name = "DataGridViewRecord";
            DataGridViewRecord.Size = new Size(387, 226);
            DataGridViewRecord.TabIndex = 34;
            // 
            // ProgressBarUpdate
            // 
            ProgressBarUpdate.BackColor = SystemColors.HighlightText;
            ProgressBarUpdate.Location = new Point(12, 811);
            ProgressBarUpdate.Name = "ProgressBarUpdate";
            ProgressBarUpdate.Size = new Size(834, 13);
            ProgressBarUpdate.TabIndex = 39;
            // 
            // ComboBoxNewDimention
            // 
            ComboBoxNewDimention.FormattingEnabled = true;
            ComboBoxNewDimention.Location = new Point(99, 513);
            ComboBoxNewDimention.Name = "ComboBoxNewDimention";
            ComboBoxNewDimention.Size = new Size(322, 23);
            ComboBoxNewDimention.TabIndex = 40;
            // 
            // LabelNewDimention
            // 
            LabelNewDimention.AutoSize = true;
            LabelNewDimention.Location = new Point(27, 516);
            LabelNewDimention.Name = "LabelNewDimention";
            LabelNewDimention.Size = new Size(66, 15);
            LabelNewDimention.TabIndex = 41;
            LabelNewDimention.Text = "Dimention:";
            // 
            // ComboBoxNewFinish
            // 
            ComboBoxNewFinish.FormattingEnabled = true;
            ComboBoxNewFinish.Location = new Point(99, 542);
            ComboBoxNewFinish.Name = "ComboBoxNewFinish";
            ComboBoxNewFinish.Size = new Size(322, 23);
            ComboBoxNewFinish.TabIndex = 42;
            // 
            // LabelNewFinish
            // 
            LabelNewFinish.AutoSize = true;
            LabelNewFinish.Location = new Point(52, 545);
            LabelNewFinish.Name = "LabelNewFinish";
            LabelNewFinish.Size = new Size(41, 15);
            LabelNewFinish.TabIndex = 43;
            LabelNewFinish.Text = "Finish:";
            // 
            // TextStatus
            // 
            TextStatus.BackColor = SystemColors.Control;
            TextStatus.BorderStyle = BorderStyle.None;
            TextStatus.Location = new Point(672, 828);
            TextStatus.Name = "TextStatus";
            TextStatus.Size = new Size(172, 16);
            TextStatus.TabIndex = 44;
            // 
            // Dupont_Price_List
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(858, 849);
            Controls.Add(TextStatus);
            Controls.Add(ComboBoxNewFinish);
            Controls.Add(LabelNewFinish);
            Controls.Add(ComboBoxNewDimention);
            Controls.Add(LabelNewDimention);
            Controls.Add(ProgressBarUpdate);
            Controls.Add(PanelUpdateField);
            Controls.Add(DataGridViewMasterDiscountList);
            Controls.Add(LabelPanel3Title);
            Controls.Add(CheckBoxUseField);
            Controls.Add(ComboBoxNewBrand);
            Controls.Add(LabelNewBrand);
            Controls.Add(ComboBoxNewWeight);
            Controls.Add(LabelNewWeight);
            Controls.Add(ComboBoxNewUPC);
            Controls.Add(LabelNewUPC);
            Controls.Add(ComboBoxNewListPrice);
            Controls.Add(LabelNewListPrice);
            Controls.Add(ComboBoxNewDescription);
            Controls.Add(LabelNewDescription);
            Controls.Add(ComboBoxNewSKU);
            Controls.Add(LabelNewSKU);
            Controls.Add(ButtonReadMasterPriceList);
            Controls.Add(ButtonReadNewPriceList);
            Controls.Add(LabelPanel2Title);
            Controls.Add(Panel2);
            Controls.Add(LabelPanel1Title);
            Controls.Add(Panel1);
            Name = "Dupont_Price_List";
            Text = "Dupont_Price_List";
            Panel1.ResumeLayout(false);
            Panel1.PerformLayout();
            Panel2.ResumeLayout(false);
            Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewMasterDiscountList).EndInit();
            PanelUpdateField.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DataGridViewRecord).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LabelCurrentItems;
        private Label LabelOnlineItems;
        private Label LabelNewPriceList;
        private TextBox TextBoxCurrentItems;
        private TextBox TextBoxNewPriceList;
        private TextBox TextBoxOnlineItems;
        private Button ButtonCurrentItems;
        private Button ButtonOnlineItems;
        private Button ButtonNewPriceList;
        private Panel Panel1;
        private Label LabelPanel1Title;
        private Label LabelPanel2Title;
        private Panel Panel2;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label LabelNewDescription;
        private Button ButtonMasterDiscountList;
        private Button ButtonCategoryList;
        private Button button3;
        private Label LabelMasterDiscountList;
        private Label LabelVendor;
        private Label LabelBrand;
        private TextBox TextBoxMasterDiscountList;
        private Button ButtonReadNewPriceList;
        private Button ButtonReadMasterPriceList;
        private Label LabelCategoryList;
        private TextBox TextBoxCategoryList;
        private Label LabelNewSKU;
        private ComboBox ComboBoxNewSKU;
        private ComboBox ComboBoxNewDescription;
        private ComboBox ComboBoxNewListPrice;
        private Label LabelNewListPrice;
        private ComboBox ComboBoxNewUPC;
        private Label LabelNewUPC;
        private ComboBox ComboBoxNewWeight;
        private Label LabelNewWeight;
        private ComboBox ComboBoxNewBrand;
        private Label LabelNewBrand;
        private CheckBox CheckBoxUseField;
        private Label LabelPanel3Title;
        private DataGridView DataGridViewMasterDiscountList;
        internal Button ButtonRetailUpdate;
        internal Button ButtonOnlineUpdate;
        private Panel PanelUpdateField;
        internal ComboBox ComboBoxVendor;
        internal ComboBox ComboBoxBrand;
        private ProgressBar ProgressBarUpdate;
        private DataGridView DataGridViewRecord;
        private ComboBox ComboBoxNewDimention;
        private Label LabelNewDimention;
        private ComboBox ComboBoxNewFinish;
        private Label LabelNewFinish;
        private TextBox TextStatus;
    }
}
