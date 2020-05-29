using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public enum MovementTypes {

    Left = 0, Right = 1, Up = 2, Down = 3, Fade = 4
}

public class MenusController : MonoBehaviour {
    public static MenusController s;
    public Transform bigDaddy, police;
    public List<menusList> menusOpened = new List<menusList>();


    //Class to keep the menu list in the List menusOpened
    public class menusList
    {
        public GameObject menuObj;
        public string menuName;
    }


    #region Awake
    void Awake()
    {
        s = this;
    }
    #endregion

    public void moveMenu(MovementTypes type, GameObject menu, string name, float newX, float newY, string dialogTextName = "", bool writeText = false, bool dont_screw_canvas = false, float delay = 0f)
    {
        if (delay == 0) {
            if (type == MovementTypes.Down)
                enterFromDown(menu, name, newX, newY, dont_screw_canvas);
            if (type == MovementTypes.Up)
                enterFromUp(menu, name, newX, newY, dont_screw_canvas);
            if (type == MovementTypes.Right)
                enterFromRight(menu, name, newX, newY);
            if (type == MovementTypes.Left)
                enterFromLeft(menu, name, newX, newY);

            if (dialogTextName != "") {
                
                menu.transform.GetComponentInChildren<DialogsTexts>().changeText(dialogTextName, writeText);

            }
        }
        else
            StartCoroutine(moveMenuRecall(type, menu, name, newX, newY, dialogTextName = "", writeText, dont_screw_canvas, delay));
    }

    IEnumerator moveMenuRecall(MovementTypes type, GameObject menu, string name, float newX, float newY, string dialogTextName = "", bool writeText = false, bool dont_screw_canvas = false, float delay = 0f) {
    //IEnumerator moveMenuRecall(string a) {
        yield return new WaitForSeconds(delay);
        moveMenu(type, menu, name, newX, newY, dialogTextName = "", writeText, dont_screw_canvas, 0);

    }

    #region AddToGuiAndMenuList
    //Add to menu List and add to the GUI
    public void addToGUIAndRepositeObject(GameObject menu, string name)
    {
        
        float xPos, yPos;

        menusList tempLecture = new menusList();
        tempLecture.menuObj = menu;

        if(name != null)
            tempLecture.menuName = name;

        menusOpened.Add(tempLecture);
        GLOBALS.s.DIALOG_ALREADY_OPENED = true;

        //Copy the readed local position
        xPos = menu.transform.localPosition.x;
        yPos = menu.transform.localPosition.y;

        //Set the Canvas of GUI was parent
        menu.transform.SetParent(bigDaddy, false);

        //Set again the local position, now in the GUI
        menu.transform.localPosition = new Vector3(xPos, yPos, 0f);
    }
    #endregion


    #region DestroyMenu
    //Destroy the menu
    //Put a name if you want to destroy a menu by name
    //or the reference to menu if you want use a close button with the reference or something like that
    public void destroyMenu(string name , GameObject myMenu)
    {
        menusList menu2Destroy = null;

        if (name != "")
        {
            menu2Destroy = forEachFindName(name);
            if(menu2Destroy != null)
            { 
               //Destroy the Gameobject
               Destroy(menu2Destroy.menuObj);
               //Remove from list
               menusOpened.Remove(menu2Destroy);
            }
            else
            {
                Debug.Log("ERROR! CANT FIND THE MENU");
            }
        }

        else if(myMenu != null)
        {
            menu2Destroy = forEachFindTheMenuItself(myMenu);
            if (menu2Destroy != null && menu2Destroy.menuObj != null)
            {
                
                Destroy(menu2Destroy.menuObj);

                menusOpened.Remove(menu2Destroy);
            }
            else
            {
                Debug.Log("ERROR! CANT FIND THE MENU");
            }
        }
        else
        {
            Debug.Log("ERROR! NO NAME OR MENU ADDED");
        }

        if(menusOpened.Count <= 0)
        {

            GLOBALS.s.DIALOG_ALREADY_OPENED = false;
        }
    }
    #endregion

    #region EnterFromLeftWithPunch
    //Make the menu enter from left
    //If newX and New Y is equals to 0, the menu will use the original position of prefab 
    void enterFromLeft(GameObject menu, string name, float newX, float newY)
    {
        float xPos, yPos;
        menu.SetActive(false);
        addToGUIAndRepositeObject(menu, name);

        //Use the passed position
        if (newX != 0)
        {
            xPos = newX;
        }
        //Use the original position
        else
        {
            xPos = menu.transform.localPosition.x;
        }

        if (newY != 0)
        {
            yPos = newY;
        }
        else
        {
            yPos = menu.transform.localPosition.y;
        }
        //Put out of the screen
        menu.transform.localPosition = new Vector3((xPos - Screen.width), yPos, 0f);
        //Move back to the screen and call punch at the end
        StartCoroutine(ActivateMenu(xPos, menu));
    }

    IEnumerator ActivateMenu(float xPos, GameObject menu)
    {
        yield return new WaitForEndOfFrame();
        menu.SetActive(true);
        menu.transform.DOLocalMoveX(xPos, 0.5f).OnComplete(() => punchLeft(menu));
    }

    void punchLeft(GameObject menu)
    {
        transform.DOPunchPosition(new Vector3(2f, 0f, 0f), 0.5f, 7, 0.9f);
    }
    #endregion

    #region EnterFromRightWithPunch
    //Make the menu enter from right
    //If newX and New Y is equals to 0, the menu will use the original position of prefab 
     void enterFromRight(GameObject menu, string name, float newX, float newY)
    {
        float xPos, yPos;
        addToGUIAndRepositeObject(menu, name);

        //Use the passed position
        if (newX != 0)
        {
            xPos = newX;
        }
        //Use the original position
        else
        {
            xPos = menu.transform.localPosition.x;
        }

        if (newY != 0)
        {
            yPos = newY;
        }
        else
        {
            yPos = menu.transform.localPosition.y;
        }

        //Put out of the screen
        menu.transform.localPosition = new Vector3((xPos + Screen.width), yPos, 0f);
        //Move back to the screen and call punch at the end
        menu.transform.DOLocalMoveX(xPos, 0.5f).OnComplete(() => punchRight(menu));
    }

    void punchRight(GameObject menu)
    {
         transform.DOPunchPosition(new Vector3(-2f, 0f, 0f), 0.5f, 7, 0.9f);
    }
    #endregion

    #region EnterFromUpWithPunch
    //Make the menu enter from up
    //If newX and New Y is equals to 0, the menu will use the original position of prefab 
     void enterFromUp(GameObject menu, string name, float newX, float newY, bool dont_screw_canvas = false)
    {
        float xPos, yPos;
        if (!dont_screw_canvas)
        {
            Debug.Log(" PUTTING OUT OF SCREEN");
            addToGUIAndRepositeObject(menu, name);
        }

        //Use the passed position
        if (newX != 0)
        {
            xPos = newX;
        }
        //Use the original position
        else
        {
            xPos = menu.transform.localPosition.x;
        }

        if (newY != 0)
        {
            yPos = newY;
        }
        else
        {
            yPos = menu.transform.localPosition.y;
        }

       
        //Put out of the screen 
        menu.transform.localPosition = new Vector3(xPos, yPos + Screen.height, 0f);
        //Move back to the screen and call punch at the end
        menu.transform.DOLocalMoveY(yPos, 0.5f).OnComplete(() => punchUp(menu));
        
    }

    void punchUp(GameObject menu)
    {
        transform.DOPunchPosition(new Vector3(0f, 2f, 0f), 0.5f, 7, 0.9f);
    }
    #endregion

    #region EnterFromDownWithPunch
    //Make the menu enter from up
    //If newX and New Y is equals to 0, the menu will use the original position of prefab 
     void enterFromDown(GameObject menu, string name, float newX, float newY, bool dont_screw_canvas = false)
    {
        float xPos, yPos;
        if (!dont_screw_canvas)
        {
            addToGUIAndRepositeObject(menu, name);
        }
        //Use the passed position
        if (newX != 0)
        {
            xPos = newX;
        }
        //Use the original position
        else
        {
            xPos = menu.transform.localPosition.x;
        }

        if (newY != 0)
        {
            yPos = newY;
        }
        else
        {
            yPos = menu.transform.localPosition.y;
        }

        if (!dont_screw_canvas) {
            //Put out of the screen
            menu.transform.localPosition = new Vector3(xPos, yPos - Screen.height, 0f);
            //Move back to the screen and call punch at the end
            menu.transform.DOLocalMoveY(yPos, 0.5f).OnComplete(() => punchDown(menu));
        }
    }

    void punchDown(GameObject menu)
    {
        transform.DOPunchPosition(new Vector3(0f, 2f, 0f), 0.5f, 7, 0.9f);
    }
    #endregion

    #region Go Out and Destroy
    public void goOutDestroy(string name, GameObject menu, string side)
    {
        #region Find the menu
        menusList menu2move = null;

        if (name != "")
        {
            menu2move = forEachFindName(name);
            if (menu2move == null)
            {
                Debug.Log("ERROR! CANT FIND THE MENU");
                return;
            }
        }

        else if (menu2move != null)
        {
            menu2move = forEachFindTheMenuItself(menu);
            if (menu2move.menuObj == null)
            {
                Debug.Log("ERROR! CANT FIND THE MENU");
                return;
            }
        }
        else
        {
            Debug.Log("ERROR! NO NAME OR MENU ADDED");
            return;
        }
        #endregion

        if (side == "up")
        {
            menu2move.menuObj.transform.DOMoveY((menu2move.menuObj.transform.position.y + Screen.height), 0.7f).OnComplete(() => destroyMenu(name, menu));
        }
        else if (side == "down")
        {
           menu2move.menuObj.transform.DOMoveY((menu2move.menuObj.transform.position.y - Screen.height), 0.7f).OnComplete(() => destroyMenu(name, menu));
        }
        else if (side == "left")
        {
            menu2move.menuObj.transform.DOMoveX((menu2move.menuObj.transform.position.x - Screen.width), 0.7f).OnComplete(() => destroyMenu(name, menu));
        }
        else
        {
            menu2move.menuObj.transform.DOMoveX((menu2move.menuObj.transform.position.x + Screen.width), 0.7f).OnComplete(() => destroyMenu(name, menu));
        }

        
    }
    #endregion

    #region Adjust alpha
    public void apearAlphaCanvasGroup(GameObject menu, string name)
    {

        addToGUIAndRepositeObject(menu, name);

        menu.GetComponent<CanvasGroup>().alpha = 0;
        //.OnComplete(() => appear(menu));
        menu.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        //Move back to the screen and call punch at the end

    }

    #endregion

    #region AdjustZOrder 
    //Just change the zorder of the menu. Call if you create a menu over the object
    public void bring2FrontZOrder(string name, GameObject closeClicked)
    {
        if (name != "")
        {
            foreach (menusList theMenu in menusOpened)
            {
                if (theMenu.menuName == name)
                {

                    if (theMenu.menuObj != null)
                    {
                        //Bring to front
                        theMenu.menuObj.transform.SetParent(police, false);
                        theMenu.menuObj.transform.SetParent(bigDaddy, false);
                        break;

                    }
                }
            }
        }
        else
        {
            Debug.Log("ERROR, NO NAME OR CLOSER ADDED");
        }
    }
    #endregion

    #region MoveRepositeTheMenuToNewPosition
    public void repositeMenu(string name, GameObject closeClicked, float newXpos, float NewYpos, float newScale = 0)
    {
        menusList menu2Move;

        if (name != "")
        {
            menu2Move = forEachFindName(name);

            menu2Move.menuObj.transform.DOLocalMove(new Vector3(newXpos, NewYpos, 0f), 1.5f);
            if(newScale !=0)
            {
                menu2Move.menuObj.transform.DOScale(new Vector3(newScale, newScale, newScale), 0f);
            }
            
        }
        else
        {

        }

    }
    #endregion

    #region ForEachFindInTheListofMenus
    menusList forEachFindName(string name2Find)
    {
        foreach (menusList theMenu in menusOpened)
        {
            if (theMenu.menuName == name2Find)
            {
                if (theMenu.menuObj != null)
                {
                    return theMenu;
                }
            }
        }
        return null;
    }

    menusList forEachFindTheMenuItself(GameObject menu)
    {

        foreach (menusList theMenu in menusOpened)
        {
            // Debug.Log(theMenu.menuName + " Meu nominho");
            if (theMenu.menuObj == menu)
            {
                //Find empty references and destroy
                if (theMenu.menuObj != null)
                {
                    return theMenu;
                }

            }
        }
        return null;
    }
    #endregion

    #region SomeMenus
    public void createCatastrophe(float time)
    {
        GLOBALS.s.DIALOG_ALREADY_OPENED = true;
        Invoke("cat", time);
    }
    void cat()
    {
        GameObject tempObject;
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Catastrophe"));
        MenusController.s.addToGUIAndRepositeObject(tempObject, "Catastrophe");
    }

    public void destroyCat()
    {
        GLOBALS.s.DIALOG_ALREADY_OPENED = false;
        MenusController.s.destroyMenu("Catastrophe",null);

        GameObject tempObject;
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/CatastropheList/CatastropheListv2"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "CatastropheList", 0, 0);

        if (GLOBALS.s.TUTORIAL_PHASE == 103 && GLOBALS.s.TUTORIAL_OCCURING == true)
        {
           // TutorialController.s.click_to_spin_again_gems();
        }
    }


    public void createLevelUp()
    {
        Invoke("lvlUp", 1.5f);
    }

    void lvlUp()
    {
        GameObject tempObject;
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/LevelUp"));
        MenusController.s.apearAlphaCanvasGroup(tempObject, "LevelUp");
        BE.BEAudioManager.SoundPlay(10);
    }

    public void createHellDiplomacy()
    {
        GLOBALS.s.DIALOG_ALREADY_OPENED = true;
        GameObject tempObject;


        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/BigScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "HellDiplomacy", 0, 0, "HellDiplomacy");


        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Satan/SatanOrderingg"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "Satan", 0, 0);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Buttons/YesBT"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "YesBT", 0, 0);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Buttons/NoBT"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "NoBT", 0, 0);
    }

    public void createHellDiplomacyNoOption()
    {
        GLOBALS.s.DIALOG_ALREADY_OPENED = true;
        GameObject tempObject;

        MenusController.s.destroyMenu("HellDiplomacy", null);
        MenusController.s.destroyMenu("Satan", null);
        MenusController.s.destroyMenu("NoBT", null);
        MenusController.s.destroyMenu("YesBT", null);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/BigScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "HellDiplomacy2", 0, 0, "HellDiplomacy2");


        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Satan/SatanBoasting"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SatanB", 0, 0);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Buttons/YesBT2"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "YesBT2", 0, 0);
    }

    public void destroyHellDiplomacyYes()
    {
        GLOBALS.s.DIALOG_ALREADY_OPENED = false;

        MenusController.s.destroyMenu("HellDiplomacy", null);
        MenusController.s.destroyMenu("Satan", null);
        MenusController.s.destroyMenu("NoBT", null);
        MenusController.s.destroyMenu("YesBT", null);
    }

    public void destroyHellDiplomacyYes2()
    {
        GLOBALS.s.DIALOG_ALREADY_OPENED = false;

        MenusController.s.destroyMenu("HellDiplomacy2", null);
        MenusController.s.destroyMenu("SatanB", null);
        MenusController.s.destroyMenu("YesBT2", null);
    }
    #endregion
}

