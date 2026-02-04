using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public static List<PcComponent> Components = new();

    public void Populate()
    {
        Components.Clear();
        print("Generating components");

        Components.Add(new Cpu(
            Id: "1",
            Name: "Core i7-13700K",
            Description: "Hybrid desktop CPU (8P+8E) for high-FPS gaming and heavy multitasking.",
            Price: 379.99f,
            Manufacturer: "Intel",
            Model: "i7-13700K",
            specs: new List<Spec> {
                new ("Cores/Threads", "8P+8E / 24"),
                new ("P-Core Base/Boost", "3.4 / 5.4 GHz"),
                new ("Socket", "LGA1700"),
                new ("TDP", "125W")
            }
        ));

        Components.Add(new MotherBoard(
            Id: "2",
            Name: "MAG Z790 TOMAHAWK",
            Description: "LGA1700 ATX with strong I/O and PCIe 5.0 x16 for 12th–14th Gen Intel CPUs.",
            Manufacturer: "MSI",
            Model: "Z790 TOMAHAWK",
            Price: 289.99f,
            Specs: new List<Spec> {
                new ("Socket", "LGA1700"),
                new ("Form Factor", "ATX"),
                new ("Memory", "DDR5"),
                new ("PCIe", "Gen 5 x16")
            }
        ));

        Components.Add(new Ram(
            Id: "3",
            Name: "Trident Z5 64GB (2x32) DDR5-6400",
            Description: "High-capacity, high-speed kit for heavy creative workloads and gaming.",
            Manufacturer: "G.SKILL",
            Model: "F5-6400J3239G32GX2-TZ5K",
            Price: 219.99f,
            Specs: new List<Spec> {
                new ("Capacity", "64 GB"),
                new ("Speed", "DDR5-6400"),
                new ("CAS", "CL32"),
                new ("Voltage", "1.40V")
            }
        ));

        Components.Add(new Gpu(
            Id: "4",
            Name: "Nvidia RTX 5090",
            Description: "High-value 1440p/entry-4K GPU with modern AV1 encode and DisplayPort 2.1.",
            Price: 499.99f,
            Manufacturer: "NVidia",
            Maker: "Nvidia",
            Model: "RTX 5090",
            Specs: new List<Spec> {
                new ("VRAM", "24 GB GDDR6"),
                new ("Game/Boost", "2.1 / 2.4 GHz"),
                new ("TBP", "263W"),
                new ("Outputs", "HDMI 2.1, 3x DP 2.1")
            }
        ));

        Components.Add(new Case(
           Id: "5",
           Name: "Obsidian Flow 500",
           Description: "High-airflow mid-tower chassis with tempered glass panels and integrated cable management.",
           Manufacturer: "Lumina Tech",
           Model: "OB-500-BK",
           Price: 139.99f,
           Specs: new List<Spec> {
        new ("Form Factor", "ATX, Micro-ATX, Mini-ITX"),
        new ("Color", "Matte Black"),
        new ("Max GPU Length", "410mm"),
        new ("Side Panel", "Tempered Glass")
           }
       ));

        Components.Add(new Psu(
            Id: "6",
            Name: "FOCUS GX-750",
            Description: "Quiet, efficient 80+ Gold PSU with full modular cables and ATX 3.0 support.",
            Manufacturer: "Seasonic",
            Model: "GX-750",
            Price: 119.99f,
            Specs: new List<Spec> {
                new ("Wattage", "750W"),
                new ("Efficiency", "80+ Gold"),
                new ("Modularity", "Fully Modular"),
                new ("ATX", "ATX 3.0")
            }
        ));

        Components.Add(new Cooler(
            Id: "7",
            Name: "NH-D15",
            Description: "Premium air cooler known for quiet operation and high thermal headroom.",
            Manufacturer: "Noctua",
            Model: "NH-D15",
            Price: 99.95f,
            Specs: new List<Spec> {
                new ("Type", "Air"),
                new ("Height", "14 mm"),
                new ("TDP Rating", "≈220W"),
                new ("Sockets", "AM5/AM4/LGA1700")
            }
        ));

        Components.Add(new Gpu(
            Id: "8",
            Name: "ASUS ROG MARS 760",
            Description: "A rare dual-GPU card featuring two GTX 760 cores on a single PCB. Outperformed the GTX Titan at launch.",
            Price: 629.99f,
            Manufacturer: "ASUS",
            Maker: "NVIDIA",
            Model: "MARS 760",
            Specs: new List<Spec> {
                new ("VRAM", "4 GB GDDR5 (2GB x 2)"),
                new ("Game/Boost", "1006 / 1072 MHz"),
                new ("TBP", "300W (2x 8-pin)"),
                new ("Outputs", "2x DVI, 1x HDMI, 1x DP")
            }
        ));

        Components.Add(new Cpu(
            Id: "9",
            Name: "AMD Ryzen 5 1600X",
            Description: "First-generation Zen architecture processor. Delivers 6 cores and 12 threads for solid multi-tasking performance on the AM4 platform.",
            Price: 149.99f,
            Manufacturer: "AMD",
            Model: "Ryzen 5 1600X",
            specs: new List<Spec> {
                new ("Socket", "AM4"),
                new ("Cores/Threads", "6C / 12T"),
                new ("Base/Boost", "3.6 / 4.0 GHz"),
                new ("TDP", "95W"),
                new ("L3 Cache", "16MB")
            }
        ));

        Components.Add(new Ram(
            Id: "10",
            Name: "Kingston HyperX Fury 16GB DDR4-3200",
            Description: "Plug N Play automatic overclocking. Features a low-profile heat spreader design compatible with large CPU air coolers.",
            Manufacturer: "Kingston",
            Model: "HX432C16FB3/16",
            Price: 49.99f,
            Specs: new List<Spec> {
                new ("Capacity", "16 GB"),
                new ("Speed", "DDR4-3200"),
                new ("CAS", "CL16"),
                new ("Voltage", "1.35V")
            }
        ));

        // ===== GPUs =====

        /*Components.Add(new Gpu(
            Id: "3",
            Name: "Radeon RX 7800 XT",
            Description: "High-value 1440p/entry-4K GPU with modern AV1 encode and DisplayPort 2.1.",
            Price: 499.99f,
            Manufacturer: "AMD",
            Maker: "AMD",
            Model: "RX 7800 XT",
            Specs: new List<Spec> {
                new Spec("VRAM", "16 GB GDDR6"),
                new Spec("Game/Boost", "2.1 / 2.4 GHz"),
                new Spec("TBP", "263W"),
                new Spec("Outputs", "HDMI 2.1, 3x DP 2.1")
            }
        ));

        // ===== Motherboards =====
        Components.Add(new MotherBoard(
            Id: "4",
            Name: "ROG STRIX B650E-F",
            Description: "AM5 ATX board with PCIe 5.0 support and robust VRMs for Ryzen 7000/8000.",
            Manufacturer: "ASUS",
            Model: "B650E-F",
            Price: 259.99f,
            Specs: new List<Spec> {
                new Spec("Socket", "AM5"),
                new Spec("Form Factor", "ATX"),
                new Spec("Memory", "DDR5"),
                new Spec("PCIe", "Gen 5 GPU/M.2")
            }
        ));

        Components.Add(new MotherBoard(
            Id: "5",
            Name: "MAG Z790 TOMAHAWK",
            Description: "LGA1700 ATX with strong I/O and PCIe 5.0 x16 for 12th–14th Gen Intel CPUs.",
            Manufacturer: "MSI",
            Model: "Z790 TOMAHAWK",
            Price: 289.99f,
            Specs: new List<Spec> {
                new Spec("Socket", "LGA1700"),
                new Spec("Form Factor", "ATX"),
                new Spec("Memory", "DDR5"),
                new Spec("PCIe", "Gen 5 x16")
            }
        ));

        // ===== RAM =====
        Components.Add(new Ram(
            Id: "6",
            Name: "Vengeance 32GB (2x16) DDR5-6000",
            Description: "Balanced DDR5 kit ideal for AM5/Intel platforms; great price-to-perf.",
            Manufacturer: "Corsair",
            Model: "CMK32GX5M2B6000",
            Price: 109.99f,
            Specs: new List<Spec> {
                new Spec("Capacity", "32 GB"),
                new Spec("Speed", "DDR5-6000"),
                new Spec("CAS", "CL36"),
                new Spec("Voltage", "1.35V")
            }
        ));

        Components.Add(new Ram(
            Id: "7",
            Name: "Trident Z5 64GB (2x32) DDR5-6400",
            Description: "High-capacity, high-speed kit for heavy creative workloads and gaming.",
            Manufacturer: "G.SKILL",
            Model: "F5-6400J3239G32GX2-TZ5K",
            Price: 219.99f,
            Specs: new List<Spec> {
                new Spec("Capacity", "64 GB"),
                new Spec("Speed", "DDR5-6400"),
                new Spec("CAS", "CL32"),
                new Spec("Voltage", "1.40V")
            }
        ));

        // ===== Drives =====
        Components.Add(new Drive(
            Id: "8",
            Name: "990 PRO 1TB NVMe",
            Description: "PCIe 4.0 M.2 SSD with top-tier sequential speeds and solid endurance.",
            Manufacturer: "Samsung",
            Model: "MZ-V9P1T0",
            Price: 119.99f,
            Specs: new List<Spec> {
                new Spec("Form Factor", "M.2 2280"),
                new Spec("Interface", "PCIe 4.0 x4 NVMe"),
                new Spec("Seq Read/Write", "7,450 / 6,900 MB/s"),
                new Spec("Endurance", "600 TBW")
            }
        ));

        Components.Add(new Drive(
            Id: "9",
            Name: "WD Blue 2TB HDD",
            Description: "Reliable mass storage for media libraries, backups, and general use.",
            Manufacturer: "Western Digital",
            Model: "WD20EZBX",
            Price: 54.99f,
            Specs: new List<Spec> {
                new Spec("Form Factor", "3.5\""),
                new Spec("Interface", "SATA III"),
                new Spec("RPM", "7200"),
                new Spec("Cache", "256 MB")
            }
        ));

        // ===== Coolers =====
        Components.Add(new Cooler(
            Id: "10",
            Name: "NH-D15",
            Description: "Premium dual-tower air cooler known for quiet operation and high thermal headroom.",
            Manufacturer: "Noctua",
            Model: "NH-D15",
            Price: 99.95f,
            Specs: new List<Spec> {
                new Spec("Type", "Air"),
                new Spec("Height", "165 mm"),
                new Spec("TDP Rating", "≈220W"),
                new Spec("Sockets", "AM5/AM4/LGA1700")
            }
        ));

        Components.Add(new Cooler(
            Id: "11",
            Name: "iCUE H100i Elite",
            Description: "240 mm AIO with RGB and good performance in compact ATX/mATX builds.",
            Manufacturer: "Corsair",
            Model: "H100i",
            Price: 149.99f,
            Specs: new List<Spec> {
                new Spec("Type", "AIO (240 mm)"),
                new Spec("Fans", "2x120 mm"),
                new Spec("RGB", "Yes"),
                new Spec("Sockets", "AM5/AM4/LGA1700")
            }
        ));

        // ===== PSUs =====


        Components.Add(new Psu(
            Id: "13",
            Name: "RM850x",
            Description: "Proven 850W platform suitable for high-end GPUs; fully modular cabling.",
            Manufacturer: "Corsair",
            Model: "RM850x (2023)",
            Price: 139.99f,
            Specs: new List<Spec> {
                new Spec("Wattage", "850W"),
                new Spec("Efficiency", "80+ Gold"),
                new Spec("Modularity", "Fully Modular"),
                new Spec("ATX", "ATX 3.0 / PCIe 5")
            }
        ));


        Components.Add(new Case(
            Id: "14",
            Name: "case",
            Description: "case",
            Manufacturer: "case",
            Model: "case",
            Price: 139.99f,
            Specs: new List<Spec> {
                new Spec("case", "case"),

        ));}*/
    }

    public static string GetComponentnameById(string id)
    {
        var comp = Components.Find(c => c.ModelId == id);
        return comp != null ? comp.Name : id;
    }

    [SerializeField]
    public List<PcComponent> ViewComponents = new();

    private void Awake()
    {
        Populate();
        ViewComponents = Components;
    }

}
