import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'artists',
})
export class ArtistsPipe implements PipeTransform {
  transform(value: string[]): string {
    return value.join(', ');
  }
}
