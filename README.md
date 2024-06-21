# Procedural Worlds

Procedural Worlds is an open-source project aimed at learning and implementing Wave Function Collapse (WFC) to generate realistic and dynamic hexagonal tile maps, similar to those seen in popular games and simulations. This project uses the Kaykit Medieval Hexagon asset pack and integrates the principles from various resources to create an engaging and educational procedural generation system.

## Project Goal

The primary goal of this project is to understand how WFC works and explore the potential of creating more realistic worlds using this algorithm. The project starts with simple road hexagons and aims to expand into more complex and realistic worlds, as depicted in the image below:

![Procedural Worlds Example](https://img.itch.zone/aW1hZ2UvMjY2ODI5MC8xNTkwNTU5Ny5qcGc=/original/x%2B5EmO.jpg)

## Resources Used

- [Red Blob Games Hexagon Grid Guide](https://www.redblobgames.com/grids/hexagons/#basics)
- [Wave Function Collapse by Maxim Gumin](https://github.com/mxgmn/WaveFunctionCollapse/)
- [Wave Function Collapse Tips and Tricks by Boris the Brave](https://www.boristhebrave.com/2020/02/08/wave-function-collapse-tips-and-tricks/)
- [Kaykit Medieval Hexagon Pack](https://kaylousberg.itch.io/kaykit-medieval-hexagon)

## Getting Started

### Prerequisites

- Unity 2022.3 or later
- Basic knowledge of C# and Unity

### Installation

- Clone the repository:
    ```bash
    git clone https://github.com/yourusername/ProceduralWorlds.git
    ```
- Open the project in Unity.
- Ensure the Kaykit Medieval Hexagon Pack is imported into your project.

### Usage

- Generate the hex map using the `HexMapGenerator`.
- Initialize the WFC using `WFCManager`.
- Start the WFC process by collapsing tiles one by one or all at once.


## Screenshots

![App Screenshot](https://raw.githubusercontent.com/I-and-Others/IAndOthers.ProceduralWorlds/main/wfc_generated.png)


## Roadmap

- [x] Implement basic WFC for road hexagons
- [ ] Expand tile set to include various terrain types (forests, rivers, mountains)
- [ ] Improve adjacency logic for WFC
- [ ] Add more complex rules for tile adjacency
- [ ] Implement fixed files
- [ ] Implement biomes and biome transitions

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

