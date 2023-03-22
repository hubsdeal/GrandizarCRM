import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'nameSplit' })
export class NameSplitPipe implements PipeTransform {
  transform(name: string): string {
    if (!name) {
      return '';
    }

    const splitNames = name.trim().split(/\s+/);
    let characters = '';

    for (const splitName of splitNames) {
      const trimmedName = splitName.trim();

      if (trimmedName) {
        characters += trimmedName.charAt(0);

        if (characters.length >= 2) {
          break;
        }
      }
    }

    return characters.toUpperCase();
  }
}