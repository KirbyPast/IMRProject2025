using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public static List<PcComponent> Components = new();

    public void Populate()
    {
        Components.Clear();

        // ===== CPUs =====
        Components.Add(new Cpu(
            Name: "Ryzen 7 7800X3D",
            Description: "8-core/16-thread AM5 gaming CPU with 3D V-Cache; excels in latency-sensitive titles.",
            Price: 399.99f,
            Manufacturer: "AMD",
            Model: "7800X3D",
            specs: new List<Spec> {
                new Spec("Cores/Threads", "8/16"),
                new Spec("Base/Boost", "4.2 / 5.0 GHz"),
                new Spec("Socket", "AM5"),
                new Spec("TDP", "120W")
            }
        ));

        Components.Add(new Cpu(
            Name: "Core i7-13700K",
            Description: "Hybrid desktop CPU (8P+8E) for high-FPS gaming and heavy multitasking.",
            Price: 379.99f,
            Manufacturer: "Intel",
            Model: "i7-13700K",
            specs: new List<Spec> {
                new Spec("Cores/Threads", "8P+8E / 24"),
                new Spec("P-Core Base/Boost", "3.4 / 5.4 GHz"),
                new Spec("Socket", "LGA1700"),
                new Spec("TDP", "125W")
            }
        ));

        // ===== GPUs =====
        Components.Add(new Gpu(
            Name: "GeForce RTX 4070",
            Description: "1440p-focused GPU with strong efficiency and DLSS 3 frame generation.",
            Price: 549.99f,
            Manufacturer: "NVIDIA",
            Maker: "NVIDIA",
            Model: "RTX 4070",
            Specs: new List<Spec> {
                new Spec("VRAM", "12 GB GDDR6X"),
                new Spec("Boost Clock", "2.48 GHz"),
                new Spec("TBP", "200W"),
                new Spec("Outputs", "HDMI 2.1, 3x DP 1.4a")
            }
        ));

        Components.Add(new Gpu(
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
            Name: "FOCUS GX-750",
            Description: "Quiet, efficient 80+ Gold PSU with full modular cables and ATX 3.0 support.",
            Manufacturer: "Seasonic",
            Model: "GX-750",
            Price: 119.99f,
            Specs: new List<Spec> {
                new Spec("Wattage", "750W"),
                new Spec("Efficiency", "80+ Gold"),
                new Spec("Modularity", "Fully Modular"),
                new Spec("ATX", "ATX 3.0")
            }
        ));

        Components.Add(new Psu(
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
    }

    [SerializeField]
    public List<PcComponent> ViewComponents = new();

    private void Awake()
    {
        Populate();
        ViewComponents = Components;
    }

}
