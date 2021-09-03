import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'artistsPipe',
})
export class ArtistsPipePipe implements PipeTransform {
  transform(value: string[]): string {
    return value.join(', ');
  }
}
