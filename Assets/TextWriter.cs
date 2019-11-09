using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour {

    public static TextWriter s;

    string text_displaying = "";
    string text_final = "";
    int k = 0;
    string cur_color = "";
    Text text_component;
    float writing_speed = 0.02f;
    float dot_delay = 0.25f;

    void Awake()
    {
        s = this;
    }

    public void write_text(Text txt = null, string txt_to_display = "", float custom_speed = 0, float custom_dot_delay = 0)
    {
        cur_color = "";
        if (custom_speed != 0)
            writing_speed = custom_speed;
        else
            writing_speed = 0.02f;

        if (custom_dot_delay != 0)
            dot_delay = custom_dot_delay;
        else dot_delay = 0.25f;

        if (txt != null)
        {
            text_component = txt;
            text_final = txt_to_display;
            text_displaying = "";
            k = 0;
            display_text();
        }
    }

    public void display_text()
    {
            int i = 0;
            //if (string.Equals("a", "a") )Debug.Log("AAAAAAAAAAAAAAA");
            if (k < text_final.Length && text_component != null)
            {
                if (string.Equals(text_final[k].ToString(), "<") == false)
                {
                    //Debug.Log("CUR CHAR: " + text_final[k]);
                    text_displaying += text_final[k++];
                }
                else //COLOR CASE
                {
                    i = 0;
                    if (string.Equals(cur_color, ""))
                    {
                        //Debug.Log(" < SYMBOL!!!!!!!! ");
                        do
                        {
                            cur_color += text_final[k + i];
                            i++;
                            // Debug.Log("< CUR CHAR: " + text_final[k+i] + " CUR COLOR: " +cur_color);
                        } while (!string.Equals(text_final[k + i].ToString(), ">"));

                        k += i;
                        if (k < text_final.Length)
                            text_displaying += cur_color + text_final[k++];
                    }
                    else
                    {
                        do
                        {
                            i++;
                        } while (!string.Equals(text_final[k + i].ToString(), ">"));

                        k += i + 1;
                        text_displaying += "</color>";

                        if (k < text_final.Length)
                            text_displaying += text_final[k++];
                        cur_color = "";
                    }
                }

                // display the text for real
                if (string.Equals(cur_color, ""))
                    text_component.text = text_displaying;
                else
                    text_component.text = text_displaying + "</color>";

                if (k < text_final.Length)
                {
                    //if the text has a dot, give it a small pause
                    if (string.Equals(text_final[k - 1].ToString(), "!") || string.Equals(text_final[k - 1].ToString(), ".") || string.Equals(text_final[k - 1].ToString(), "?"))
                        Invoke("display_text", dot_delay);
                    else
                        Invoke("display_text", writing_speed);
                }
                //Invoke("display_text", 0.1f);

            }
        }
    }

