
# FaceSight
<div style="text-align: justify"> We present FaceSight, a computer vision-based hand-to-face gesture
sensing technique for AR glasses. FaceSight fxes an infrared camera
onto the bridge of AR glasses to provide extra sensing capability of
the lower face and hand behaviors.We obtained 21 hand-to-face gestures
and demonstrated the potential interaction benefts through
fve AR applications. We designed and implemented an algorithm
pipeline that segments facial regions, detects hand-face contact (f1 score: 98.36%), and trains convolutional neural network (CNN) models
to classify the hand-to-face gestures. The input features include
gesture recognition, nose deformation estimation, and continuous
fngertip movement. Our algorithm achieves classifcation accuracy
of all gestures at 83.06%, proved by the data of 10 users. Due to the
compact form factor and rich gestures, we recognize FaceSight as a
practical solution to augment input capability of AR glasses in the
future. </div>


## Application
![demo](./demo.gif)

The application is built on [Nreal Light AR glasses](https://www.nreal.ai/light/) which runs Android operating system. Using nrsdk for development in unity and vysor for distribution.
