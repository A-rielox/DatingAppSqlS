import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';

@Component({
   selector: 'app-photo-editor',
   templateUrl: './photo-editor.component.html',
   styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
   @Input() member: Member | undefined;

   constructor() {}

   ngOnInit(): void {}

   setMainPhoto(photo: Photo) {}

   deletePhoto(photoId: number) {}
}
