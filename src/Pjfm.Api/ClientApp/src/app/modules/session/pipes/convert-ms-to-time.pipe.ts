import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'convertMsToTime',
})
export class ConvertMsToTimePipe implements PipeTransform {
  transform(value: number): string {
    const seconds = Math.floor(value / 1000);
    const minutes = Math.floor(seconds / 60);
    const leftOverSeconds = seconds - minutes * 60;

    return `${Math.round(minutes)}:${leftOverSeconds < 10 ? '0' : ''}${Math.round(leftOverSeconds)}`;
  }
}
