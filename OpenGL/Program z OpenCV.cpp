#include "stdafx.h"
#include <cv.h>
#include <highgui.h>


void main(void)
{
	IplImage *obrazek=cvLoadImage("pomiar.jpeg"); //wczytanie obrazu znajduj¹cego siê w bie¿¹cym katalogu programu
	cvSmooth( obrazek, obrazek, CV_GAUSSIAN, 8, 8 );
	cvShowImage("CannyTest",obrazek);               //wyœwietlenie wczytanego obrazu
	 
	cvWaitKey();                                  //wcisniêcie dowolnego klawisza koñczy dzia³anie programu
}