

export function clickOutside(
  node: HTMLElement,
  callback: () => void
): { destroy(): void } {
  const handleClick = (event: MouseEvent) => {
    if (!node.contains(event.target as Node)) {
      callback();
    }
  };

  document.addEventListener("click", handleClick, true);

  return {
    destroy() {
      document.removeEventListener("click", handleClick, true);
    },
  };
}

export function formatRelativeTime(date: string): string {
  const now = new Date();
  const dateObj = new Date(date);
  const diff = dateObj.getTime() - now.getTime();
  const seconds = Math.round(diff / 1000);

  const rtf = new Intl.RelativeTimeFormat("en", { numeric: "auto" });

  const divisions = [
    { amount: 60, name: "minute" },
    { amount: 60, name: "hour" },
    { amount: 24, name: "day" },
    { amount: 7, name: "week" },
    { amount: 4.34524, name: "month" },
    { amount: 12, name: "year" },
    { amount: Number.POSITIVE_INFINITY, name: "year" },
  ];

  let unit = "second";
  let value = seconds;

  for (const division of divisions) {
    if (Math.abs(value) < division.amount) {
      break;
    }
    value /= division.amount;
    unit = division.name;
  }

  return rtf.format(Math.round(value), unit as Intl.RelativeTimeFormatUnit);
}