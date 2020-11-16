// using UnityEngine;
// using System.Collections;
// using System.IO;
//
// [System.Serializable]
// public class Game
// {
//     private Save GreateSaveGameObject() {
//       Save save = new Save();
//       // save positions of line
//       save.origLineComponent.GetPositions(save.newPos);
//       save.savedPoints.Add(save.newPos);
//       // save color of line
//       Color color = save.origLineComponent.GetColor();
//       save.colors.Add(color);
//       // save textures of line
//       string texture = save.origLineComponent.GetTexture();
//       save.textures.Add(texture);
//       // save size of line
//       float width = save.origLineComponent.GetWidth();
//       save.sizes.Add(width);
//       // save opacity of line
//       float opacity = save.origLineComponent.GetOpacity();
//       save.opacities.Add(opacity);
//
//       return save;
//     }
//
//     public void SaveGame() {
//       Save save = CreateSaveGameObject();
//       //use data instead in file
//       BinaryFormatter bf = new BinaryFormatter();
//       FileStream file = File.Create(Application.persistentDataPath + "/drawing.save");
//       bf.Serialize(file, save);
//       file.Close();
//
//       // screen shot scene and send to backend so Connor can fetch and display preview in main end
//       // beta release
//
//       Debug.Log("Drawing Saved");
//     }
//
//     public void LoadGame() {
//       if (File.Exists(Application.persistentDataPath + "/drawing.save")) {
//         //NEED TO RESET DRAWING FIRST
//
//         BinaryFormatter bf = new BinaryFormatter();
//         Filestream file = File.Open(Application.persistentDataPath + "/drawing.save", FileMode.Open);
//         Save save = (Save)bf.Deserialize(false);
//         file.Close();
//         Deebug.Log("Drawing Opened");
//
//         // convert drawing information into game state
//         int i = 0;
//         for (i = 0; i <  save.numLines; ++i) {
//           save.newLine.SetColor(save.colors[i]);
//           save.newLine.SetTexture(save.textures[i]);
//           save.newLine.SetWidth(save.sizes[i]);
//           save.newLine.SetOpacity(save.opacities[i]);
//           save.newLine.GetComponent<LineRenderer>().SetPositions(save.savedPoints[i]);
//         }
//
//       }
//     }
// }
